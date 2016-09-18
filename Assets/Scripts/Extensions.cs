using UnityEngine;
using System.Collections;

namespace ExtensionMethods
{
	public static class MyExtensions
	{
		public static Vector2 xy(this Vector3 vec)
		{
			return new Vector2(vec.x, vec.y);
		}
		public static T[] slowAdded<T>(this T[] array, T obj)
		{
			//returns a copy of the array with obj at the end.
			T[] output = new T[array.Length + 1];
			for (int i = 0; i < array.Length; i++) {
				output[i] = array[i];
			}
			output[output.Length - 1] = obj;
			return output;
		}
		public static T[] cleaned<T>(this T[] array)
		{
			//returns a copy of the array with any null values removed. 
			int count = 0;
			for(int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					count++;
				}
			}
			T[] output = new T[array.Length - count];
			count = 0;
			for(int i = 0; i < array.Length; i++)
			{
				if(array[i] == null)
				{
					count++;
				}
				else
				{
					output[i - count] = array[i];
				}
			}
			return output;
		}
	}
}