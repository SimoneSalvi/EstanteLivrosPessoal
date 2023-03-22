using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstanteLivrosPessoal
{
    internal class Author
    {
        public   string Name { get; set; }
        public string LastName { get; set; }

        public Author()
        {

        }

        public override string ToString()
        {
            return $"{Name}|{LastName}";
        }
    }
}
