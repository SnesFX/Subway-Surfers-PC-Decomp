using System;
using System.Collections.Generic;

[Serializable]
public class BMGlyph
{
	public struct Kerning
	{
		public int previousChar;

		public int amount;
	}

	public int index;

	public int x;

	public int y;

	public int width;

	public int height;

	public int offsetX;

	public int offsetY;

	public int advance;

	public List<Kerning> kerning;

	public int GetKerning(int previousChar)
	{
		if (this.kerning != null)
		{
			int i = 0;
			for (int count = this.kerning.Count; i < count; i++)
			{
				Kerning kerning = this.kerning[i];
				if (kerning.previousChar == previousChar)
				{
					return kerning.amount;
				}
			}
		}
		return 0;
	}

	public void SetKerning(int previousChar, int amount)
	{
		if (kerning == null)
		{
			kerning = new List<Kerning>();
		}
		for (int i = 0; i < kerning.Count; i++)
		{
			if (kerning[i].previousChar == previousChar)
			{
				Kerning value = kerning[i];
				value.amount = amount;
				kerning[i] = value;
				return;
			}
		}
		Kerning item = default(Kerning);
		item.previousChar = previousChar;
		item.amount = amount;
		kerning.Add(item);
	}

	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		int num = x + width;
		int num2 = y + height;
		if (x < xMin)
		{
			int num3 = xMin - x;
			x += num3;
			width -= num3;
			offsetX += num3;
		}
		if (y < yMin)
		{
			int num4 = yMin - y;
			y += num4;
			height -= num4;
			offsetY += num4;
		}
		if (num > xMax)
		{
			width -= num - xMax;
		}
		if (num2 > yMax)
		{
			height -= num2 - yMax;
		}
	}
}
