using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

internal class FileUtil
{
	private static bool ArraysAreEqual<T>(T[] a, T[] b)
	{
		if (a == null && b == null)
		{
			return true;
		}
		if (a.Length != b.Length)
		{
			return false;
		}
		for (int i = 0; i < a.Length; i++)
		{
			if (!object.Equals(a[i], b[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static Dictionary<E, int> ReadEnumIntDictionary<E>(BinaryReader reader)
	{
		int num = reader.ReadInt32();
		Dictionary<E, int> dictionary = new Dictionary<E, int>(num);
		Type typeFromHandle = typeof(E);
		for (int i = 0; i < num; i++)
		{
			string value = reader.ReadString();
			int value2 = reader.ReadInt32();
			E key = (E)Enum.Parse(typeFromHandle, value, true);
			dictionary[key] = value2;
		}
		return dictionary;
	}

	public static void WriteEnumIntDictionary<E>(BinaryWriter writer, Dictionary<E, int> dict)
	{
		writer.Write(dict.Count);
		foreach (KeyValuePair<E, int> item in dict)
		{
			string name = Enum.GetName(typeof(E), item.Key);
			writer.Write(name);
			writer.Write(item.Value);
		}
	}

	public static Dictionary<E, string> ReadEnumStringDictionary<E>(BinaryReader reader)
	{
		int num = reader.ReadInt32();
		Dictionary<E, string> dictionary = new Dictionary<E, string>(num);
		Type typeFromHandle = typeof(E);
		for (int i = 0; i < num; i++)
		{
			string value = reader.ReadString();
			string value2 = reader.ReadString();
			if (Enum.IsDefined(typeFromHandle, value))
			{
				E key = (E)Enum.Parse(typeFromHandle, value, true);
				dictionary[key] = value2;
			}
		}
		return dictionary;
	}

	public static void WriteEnumStringDictionary<E>(BinaryWriter writer, Dictionary<E, string> dict)
	{
		writer.Write(dict.Count);
		foreach (KeyValuePair<E, string> item in dict)
		{
			string name = Enum.GetName(typeof(E), item.Key);
			writer.Write(name);
			writer.Write(item.Value);
		}
	}

	public static byte[] Load(string path, string secret)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(secret);
		FileStream input = new FileStream(path, FileMode.Open);
		BinaryReader binaryReader = new BinaryReader(input);
		int count = binaryReader.ReadInt32();
		byte[] b = binaryReader.ReadBytes(count);
		int count2 = binaryReader.ReadInt32();
		byte[] array = binaryReader.ReadBytes(count2);
		binaryReader.Close();
		byte[] array2 = new byte[bytes.Length + array.Length];
		Array.Copy(bytes, array2, bytes.Length);
		Array.Copy(array, 0, array2, bytes.Length, array.Length);
		SHA1 sHA = SHA1.Create();
		byte[] a = sHA.ComputeHash(array2);
		if (!ArraysAreEqual(a, b))
		{
			throw new IOException("Data is corrupted");
		}
		return array;
	}

	public static void Save(string path, string secret, byte[] data, int offset, int length)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(secret);
		byte[] array = new byte[bytes.Length + length];
		Array.Copy(bytes, array, bytes.Length);
		Array.Copy(data, offset, array, bytes.Length, length);
		SHA1 sHA = SHA1.Create();
		byte[] array2 = sHA.ComputeHash(array);
		FileStream fileStream = new FileStream(path, FileMode.Create);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		binaryWriter.Write(array2.Length);
		binaryWriter.Write(array2);
		binaryWriter.Write(length);
		binaryWriter.Write(data, offset, length);
		fileStream.Close();
	}
}
