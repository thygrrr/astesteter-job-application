using Unity.Mathematics;
using UnityEngine;

namespace Jovian.Tiger.Math
{
    public static class MathEx
    {
        public static float3 ProjectOnPlane(this float3 vector, float3 planeNormal)
        {
            var num1 = math.dot(planeNormal, planeNormal);
            if ((double) num1 < (double) float.Epsilon)
                return vector;
        
            var num2 = math.dot(vector, planeNormal);
        
            return new float3(vector.x - planeNormal.x * num2 / num1, vector.y - planeNormal.y * num2 / num1, vector.z - planeNormal.z * num2 / num1);
        }
        
        public static double Angle(double3 from, double3 to)
        {
            var dot = math.dot(math.normalizesafe(from), math.normalize(to));
            return math.acos(dot < -1.0 ? -1.0 : dot > 1.0 ? 1.0 : dot);
        }


        //Ray Sphere intersection
        public static bool RaySphereIntersection(float3 rayOrigin, float3 rayDirection, float3 sphereCenter, float sphereRadius, out float3 point)
        {
            float distance = 0;
            var vector = rayOrigin - sphereCenter;
            var num1 = math.dot(vector, rayDirection);
            var num2 = math.dot(vector, vector) - sphereRadius * sphereRadius;
            if (num2 > 0.0 && num1 > 0.0)
            {
                distance = 0.0f;
                point = sphereCenter;
                return false;
            }

            var num3 = num1 * num1 - num2;
            if (num3 < 0.0)
            {
                distance = 0.0f;
                point = sphereCenter;
                return false;
            }

            distance = -num1 - math.sqrt(num3);
            point = rayOrigin + rayDirection * distance;
            return true;
        }
        
        public static float3 ClosestPointOnOrToSphere(float3 r0, float3 rd, float3 s0, float radius)
        {
            var s0_r0 = s0-r0;
            var t = math.dot(s0_r0, rd);
            var p = r0 + t * rd;

            var y = math.length(p-s0);

            if (t < 0) return r0; //TODO: Do vertical projection down - this isn't on the sphere
            
            if (y < radius)
            {
                var x = math.sqrt(radius * radius - y * y);
                var t1 = math.max(t - x, 0);
                //float t2 = (t + x);
                //float thickness = (t2 - t1) / 2.0f;
                
                return r0 + rd * t1;
                //float3 p2 = r0 + rd * t2;
            }
            else
            {
                return r0 + rd * t;
            }
        }
    }
}

/*
Written by Tiger Blue in 2022

This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org>
*/