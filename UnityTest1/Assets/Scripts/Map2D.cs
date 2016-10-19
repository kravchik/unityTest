using System;
using System.Collections.Generic;

namespace Yk
{
    public class Map2D
    {
        public List<List<List<IAabb>>> Data;
        public int Width;
        public int Height;
        public float ElementSize = 1.5f;


        public Map2D(int w, int h, float elementSize)
        {
            this.Width = w;
            this.Height = h;
            Data = new List<List<List<IAabb>>>();
            this.ElementSize = elementSize;

            for (int x = 0; x < w; x++)
            {
                List<List<IAabb>> column = new List<List<IAabb>>();
                Data.Add(column);
                for (int y = 0; y < h; y++)
                {
                    List<IAabb> row = new List<IAabb>();
                    column.Add(row);
                }
            }
        }

        public IntRect calcArea(FloatRect floatRect)
        {
            var result = new IntRect(
                (int) Math.Floor(floatRect.l / ElementSize),
                (int) Math.Floor(floatRect.b / ElementSize),
                (int) Math.Floor(floatRect.r / ElementSize),
                (int) Math.Floor(floatRect.t / ElementSize)
            );
            if (result.l < 0) result.l = 0;
            if (result.b < 0) result.b = 0;
            if (result.r >= Width) result.r = Width - 1;
            if (result.t >= Height) result.t = Height - 1;

            return result;
        }

        public void Put(IAabb element)
        {
            Put(element, calcArea(element.GetAabb()));
        }

        private void Put(IAabb element, IntRect area)
        {
            for (int x = area.l; x <= area.r; x++)
            {
                for (int y = area.b; y <= area.t; y++)
                {
                    if (Data[x][y].Contains(element)) throw new Exception("trying to add but there is already such an element " + area);
                    Data[x][y].Add(element);
                    element.GetCache().Add(Data[x][y]);
                }
            }
        }

        public void UpdatePos(IAabb element)
        {
            var oldArea = calcArea(element.GetAabb());
            element.UpdateAabb();
            var newArea = calcArea(element.GetAabb());
            if (!newArea.Equals(oldArea))
            {
                Remove(element, oldArea);
                Put(element);
            }
        }

        public void Remove(IAabb element)
        {
            Remove(element, calcArea(element.GetAabb()));
        }

        private void Remove(IAabb element, IntRect area)
        {
            for (int x = area.l; x <= area.r; x++)
            {
                for (int y = area.b; y <= area.t; y++)
                {
                    if (!Data[x][y].Contains(element)) throw new Exception("trying to delete but there is no such an element " + area);
                    Data[x][y].Remove(element);
                    element.GetCache().Remove(Data[x][y]);
                }
            }
        }

        public HashSet<IAabb> GetIntersections(FloatRect src)
        {
            var area = calcArea(src);
            HashSet<IAabb> result = new HashSet<IAabb>();

            for (int x = area.l; x <= area.r; x++)
            {
                for (int y = area.b; y <= area.t; y++)
                {
                    List<IAabb> aa = Data[x][y];
                    foreach (var a in aa) result.Add(a);
                }
            }
            return result;
        }

    }

    public class IntRect
    {
        public int l;
        public int r;
        public int t;
        public int b;

        public IntRect(int l, int b, int r, int t)
        {
            this.l = l;
            this.r = r;
            this.t = t;
            this.b = b;
        }

        public bool Equals(IntRect other)
        {
            return l == other.l && r == other.r && t == other.t && b == other.b;
        }

        public override string ToString()
        {
            return l + " " + r + " " + t + " " + b;
        }
    }

    public interface IAabb
    {
        FloatRect GetAabb();
        void UpdateAabb();
        HashSet<List<IAabb>> GetCache();
    }


}


