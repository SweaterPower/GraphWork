using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditGraph
{
    public class EdgeWirth
    {
        public VertexWirth Id;

        public EdgeWirth Next;

        public int Weight = 0;

        public bool Direct = false;

        public EdgeWirth GetEnd()
        {
            if (Next == null) return this;
            return Next.GetEnd();
        }

        public EdgeWirth(VertexWirth id)
        {
            Id = id;
        }

        public EdgeWirth(VertexWirth id, EdgeWirth next)
        {
            Id = id;
            Next = next;
        }

        public EdgeWirth(VertexWirth id, int weight) : this(id)
        {
            Weight = weight;
        }

        public EdgeWirth(VertexWirth id, EdgeWirth next, int weight) : this(id, next)
        {
            Weight = weight;
        }

        public EdgeWirth(VertexWirth id, int weight, bool direct) : this(id, weight)
        {
            Direct = direct;
        }

        public EdgeWirth(VertexWirth id, EdgeWirth next, int weight, bool direct) : this(id, next, weight)
        {
            Direct = direct;
        }
    }
}
