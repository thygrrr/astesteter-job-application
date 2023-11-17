using System;
using UnityEngine;

namespace Tiger.Util
{
	[Serializable]
	public struct RangedFloat
	{
		public float minimum;
		public float maximum;

		public float Clamp(float value)
		{
			return Mathf.Clamp(value, minimum, maximum);
		}
		
		public float ClosestDelta(float current, float value)
		{
			var midpoint = (maximum + minimum)/2.0f;
			
			var combined = (current+value) % 360.0f;
			
			if (!Inside(combined))
			{
				//Closest point past the pivot point from both directions
				if (combined > midpoint + 180 || combined < midpoint - 180)
					return -value;
			}
			
			return value;
		}

		public bool Inside(float value)
		{
			return value >= minimum && value <= maximum;
		}
		
		public float Random()
		{
			return UnityEngine.Random.Range(minimum, maximum);
		}
	}
}


/*
Written by Tiger Blue in 2018

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