using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstanteLivrosPessoal
{
    internal class Book
    {
        public string Title { get; set; }
        public int Edition { get; set; }

//        public List<Author>  Author { get; set; }
        public Author Author { get; set; }

        public string Isbn { get; set; }

        public bool Reading { get; set; }

        public bool Borrowed { get; set; }

        public Book()
        {

        }

        public override string ToString()
        {
            return $"{Title}|{Edition}|{Author}|{Isbn}|{Reading}|{Borrowed}";
        }
    }


}
