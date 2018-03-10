using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditGraph
{
    class VertexWirth
    {
        public readonly int Key;

        public int Value;

        int count = 0;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                if (value >= 0)
                    count = value;
            }
        }

        public VertexWirth Next;

        public EdgeWirth Trail;

        public VertexWirth GetEnd()
        {
            if (Next == null) return this;
            return Next.GetEnd();
        }

        public VertexWirth(int key, int count, VertexWirth next, EdgeWirth trail)
        {
            Key = key;
            this.count = count;
            Next = next;
            Trail = trail;
        }

        public VertexWirth(int key, int value)
        {
            Key = key;
            Value = value;
        }
    }
}
