using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Entities;

namespace E7.ECS.LineRenderer
{
    [ExecuteAlways]
    [UpdateInGroup(typeof(LineRendererSimulationGroup))]
    public class LineSegmentTransformSystem : JobComponentSystem
    {
        EntityQuery lineSegmentQuery;
        EntityQuery billboardCameraQuery;

        protected override void OnCreate()
        {
            lineSegmentQuery = GetEntityQuery(
                ComponentType.ReadOnly<LineSegment>(),
                ComponentType.ReadWrite<Translation>(),
                ComponentType.ReadWrite<Rotation>(),
                ComponentType.ReadWrite<NonUniformScale>()
            );
            billboardCameraQuery = GetEntityQuery(
                ComponentType.ReadOnly<BillboardCamera>(),
                ComponentType.ReadWrite<LocalToWorld>()
            );
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var linePositioningJobHandle = new LinePositioningJob
            {
                cameraPos = InputManager.userPos,
                lastSystemVersion = LastSystemVersion,
                lineSegmentType = GetComponentTypeHandle<LineSegment>(isReadOnly: true),
                ltwType = GetComponentTypeHandle<LocalToWorld>(isReadOnly: true),
                translationType = GetComponentTypeHandle<Translation>(isReadOnly: false),
                rotationType = GetComponentTypeHandle<Rotation>(isReadOnly: false),
                scaleType = GetComponentTypeHandle<NonUniformScale>(isReadOnly: false),
            }.Schedule(lineSegmentQuery, inputDeps);

            return linePositioningJobHandle;
        }

        [BurstCompile]
        struct LinePositioningJob : IJobChunk
        {
            public float3 cameraPos;
            [ReadOnly] public ComponentTypeHandle<LineSegment> lineSegmentType;
            public uint lastSystemVersion;

            [ReadOnly] public ComponentTypeHandle<LocalToWorld> ltwType;

            public ComponentTypeHandle<Translation> translationType;
            public ComponentTypeHandle<Rotation> rotationType;
            public ComponentTypeHandle<NonUniformScale> scaleType;

            public void Execute(ArchetypeChunk ac, int chunkIndex, int firstEntityIndex)
            {
                //Do not commit to change if possible

                //bool lineChunkChanged = ac.DidChange(lineSegmentType, lastSystemVersion);
                //bool cameraMovedOrRotated = cameraAca.Length != 0 && cameraAca[0].DidChange(ltwType, lastSystemVersion);

                //if (!lineChunkChanged && !cameraMovedOrRotated) return;

                //These gets will commit a version bump
                var segs = ac.GetNativeArray(lineSegmentType);
                var trans = ac.GetNativeArray(translationType);
                var rots = ac.GetNativeArray(rotationType);
                var scales = ac.GetNativeArray(scaleType);

                for (int i = 0; i < segs.Length; i++)
                {
                    var seg = segs[i];

                    var tran = trans[i];
                    var rot = rots[i];
                    var scale = scales[i];

                    if (seg.from.Equals(seg.to))
                    {
                        continue;
                    }

                    float3 forward = seg.to - seg.from;

                    //We will use length too so not using normalize here
                    float lineLength = math.length(forward);
                    float3 forwardUnit = forward / lineLength;

                    //Billboard rotation
                    quaternion rotation = quaternion.identity;
                    float3 toCamera = math.normalize(cameraPos - seg.from);

                    //If forward and toCamera is collinear the cross product is 0
                    //and it will gives quaternion with tons of NaN
                    //So we rather do nothing if that is the case
                    if ((seg.from.Equals(cameraPos) ||
                            math.cross(forwardUnit, toCamera).Equals(float3.zero)) == false)
                    {
                        //This is wrong because it only taken account of the camera's position, not also its rotation.
                        rotation = quaternion.LookRotation(forwardUnit, toCamera);
                        //Debug.Log($"ROTATING {rotation} to {cameraTranslation}");
                    }

                    trans[i] = new Translation {Value = seg.from};
                    rots[i] = new Rotation {Value = rotation};
                    scales[i] = new NonUniformScale {Value = math.float3(seg.lineWidth, 1, lineLength)};
                }
            }
        }
    }
}