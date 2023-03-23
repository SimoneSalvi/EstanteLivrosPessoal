using System.ComponentModel.Design;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using EstanteLivrosPessoal;
using static System.Net.Mime.MediaTypeNames;

internal class Program
{
    private static void Main(string[] args)
    {
        int op;
        bool flag = false;
        //        List<Author> authorList = new List<Author>();
        List<Book> bookList = new List<Book>();

        Console.WriteLine("\n §§§§§§ ° ARQUIVO DE LIVROS ° §§§§§§§ \n");
        do
        {
            op = Menu();
            switch (op)
            {
                case 0:
                    Console.WriteLine("\n §§§§§§ ° fim do programa ° §§§§§§§ \n");
                    System.Environment.Exit(0);
                    flag = true;
                    break;
                case 1:
                    bookList.Add(CreateBook());
                    break;
                case 2:
                    PrintBookList(bookList);
                    break;
                case 3:
                    LerSepararLista(bookList);
                    bookList.Clear();
                    break;
                case 4:
                    PrintBookList(EstanteParaLista());
                    break;
                case 5:
                    PrintBookList(LeituraParaLista());
                    break;
                case 6:
                    PrintBookList(EmprestadosParaLista());
                    break;
                case 7:
                    Console.WriteLine("\nQual arquivo você quer editar?" +
                                      "\n   [1 - Estante]" +
                                      "\n   [2 - Leitura]" +
                                      "\n   [3 - Emprestado]");
                    int n = int.Parse(Console.ReadLine());
                    if (n == 1)
                    {
                        EstanteParaLista();
                    }
                    if (n == 2)
                    {
                        LeituraParaLista();
                    }
                    if (n == 3)
                    {
                        PrintBookList(EmprestadosParaLista());
                    }
                    EditBook(FindBook());
                    //LerSepararLista(bookList);
                    bookList.Clear();
                    break;

            }

        } while (!flag);

        // criação de objeto livro e inserção na lista
        int Menu()
        {
            Console.WriteLine("\n\nDigite a opção:\n");
            Console.WriteLine("     0 - Sair");
            Console.WriteLine("     1 - Adicionar novo livro a estante");
            Console.WriteLine("     2 - Exibir lista de livros");
            Console.WriteLine("     3 - Gravar dados da lista nos Arquivos e LIMPAR lista");
            Console.WriteLine("     4 - Arquivo dos livros da estante");
            Console.WriteLine("     5 - Arquivo dos livros que estou lendo");
            Console.WriteLine("     6 - Arquivo dos livros emprestados");
            Console.WriteLine("     7 - Alternar livro entre os arquivos");
            op = int.Parse(Console.ReadLine());
            return op;
        }

        Book CreateBook()
        {
            Book book = new Book();
            Author author = new Author();

            Console.WriteLine("Digite o título do livro:");
            book.Title = Console.ReadLine();
            Console.WriteLine("Digite a edição do livro:");
            book.Edition = int.Parse(Console.ReadLine());
            Console.WriteLine("Digite o ISBN do livro:");
            book.Isbn = Console.ReadLine();
            Console.WriteLine("O livro está: \n[1 - na estante]\n[2 - em leitura]\n[3 - emprestado]");
            int op = int.Parse(Console.ReadLine());
            if (op == 1)
            {
                book.Reading = false;
                book.Borrowed = false;
            }
            if (op == 2)
            {
                book.Reading = true;
                book.Borrowed = false;
            }
            if (op == 3)
            {
                book.Reading = false;
                book.Borrowed = true;
            }
            Console.WriteLine("Digite o nome do autor:");
            author.Name = Console.ReadLine();
            Console.WriteLine("Digite o sobrenome do autor");
            author.LastName = Console.ReadLine();
            book.Author = author;

            /*
             * Console.WriteLine("Quantos autores o livro tem?");
            op = int.Parse(Console.ReadLine());
             * for(int i = 0; i < op; i++)
            {
                Console.WriteLine($"Digite o nome do {i + 1}° autor");
                author.Name = Console.ReadLine();
                Console.WriteLine($"Digite o sobrenomenome do {i + 1}° autor");
                author.LastName = Console.ReadLine();
                authorList.Add(author);
            }
            book.Author = authorList;*/
            //bookList.Add(book);
            return book;
        }

        void PrintBookList(List<Book> l)
        {
            foreach (Book book in l)
            {
                Console.WriteLine(book.ToString());

                /*Console.WriteLine(book.Title);
                Console.WriteLine(book.Edition);
                Console.WriteLine(book.Isbn);
                Console.WriteLine(book.Author.Name);
                Console.WriteLine(book.Author.LastName);*/

                /*foreach (Author author in book.Author)
                {
                    Console.WriteLine(author.Name);
                    Console.WriteLine(author.LastName);
                }*/
            }
        }

        // criação e manipulação dos arquivos

        void LerSepararLista(List<Book> l)
        {
            foreach (Book book in l)
            {
                if (book.Borrowed == false && book.Reading == false)
                {
                    string aux = book.ToString();
                    WriteFileEstante(aux);
                }
                if (book.Borrowed == false && book.Reading == true)
                {
                    string aux = book.ToString();
                    WriteFileLeitura(aux);
                }
                if (book.Borrowed == true && book.Reading == false)
                {
                    string aux = book.ToString();
                    WriteFileEmprestado(aux);
                }
            }
        }

       /*void SobrescreverArquivoEstante(List<Book> l)
        {
            foreach (Book book in l)
            {
                
                StreamWriter sw = new("livrosEstante.txt");
                string texto = book.ToString();
                sw.WriteLine(texto);
                sw.Close();

            }
            File.WriteAllText("livrosEstante.txt", )
            Console.WriteLine("Arquivo Estante atualizado com sucesso com Sucesso!");
        }*/

        void WriteFileEstante(string texto)
        {
            try
            {
                if (File.Exists("livrosEstante.txt"))
                {
                    var aux = ReadFile("livrosEstante.txt");
                    StreamWriter sw = new StreamWriter("livrosEstante.txt");
                    sw.WriteLine(aux + texto);
                    sw.Close();
                }
                else
                {
                    StreamWriter sw = new("livrosEstante.txt");
                    sw.WriteLine(texto);
                    sw.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Console.WriteLine("Registro Gravado com Sucesso!");
                Thread.Sleep(1000);
            }
        }

        void WriteFileLeitura(string texto)
        {
            try
            {
                if (File.Exists("livrosLeitura.txt"))
                {
                    var aux = ReadFile("livrosLeitura.txt");
                    StreamWriter sw = new StreamWriter("livrosLeitura.txt");
                    sw.WriteLine(aux + texto);
                    sw.Close();
                }
                else
                {
                    StreamWriter sw = new("livrosLeitura.txt");
                    sw.WriteLine(texto);
                    sw.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Console.WriteLine("Registro Gravado com Sucesso!");
                Thread.Sleep(1000);
            }
        }

        void WriteFileEmprestado(string texto)
        {
            try
            {
                if (File.Exists("livrosEmprestado.txt"))
                {
                    var aux = ReadFile("livrosEmprestado.txt");
                    StreamWriter sw = new StreamWriter("livrosEmprestado.txt");
                    sw.WriteLine(aux + texto);
                    sw.Close();
                }
                else
                {
                    StreamWriter sw = new("livrosEmprestado.txt");
                    sw.WriteLine(texto);
                    sw.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Console.WriteLine("Registro Gravado com Sucesso!");
                Thread.Sleep(1000);
            }
        }

        string ReadFile(string s)
        {
            {
                StreamReader sr = new StreamReader(s);
                string text = "";
                try
                {
                    text = sr.ReadToEnd();
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    sr.Close();
                }
                return text;
            }
        }

        // Leitura dos arquivos retornando uma lista
        List<Book> EstanteParaLista()
        {
            bookList.Clear();
            StreamReader sr = new("livrosEstante.txt");
            string txt = "";
            string aux = "";


            for (int i = 0; sr.EndOfStream == false; i++)
            {
                Book book = new Book();
                Author author = new Author();
                //$"{Title}|{Edition}|{Author}|{Isbn}|{Reading}|{Borrowed}";

                txt = sr.ReadLine();

                //Console.WriteLine(txt);

                string[] vet = txt.Split('|');

                //Console.WriteLine($"{vet[0]}   {vet[1]}   {vet[2]}  {vet[3]}   {vet[4]}  {vet[5]}  {vet[6]}");

                string s = vet[0];
                book.Title = s;

                int n = int.Parse(vet[1]);
                book.Edition = n;

                s = vet[2];
                author.Name = s;
                book.Author = author;

                s = vet[3];
                author.LastName = s;
                book.Author = author;

                s = vet[4];
                book.Isbn = s;

                if (vet[5].Contains('t'))
                {
                    book.Reading = true;
                }
                if (vet[5].Contains('f'))
                {
                    book.Reading = false;
                }

                if (vet[6].Contains('t'))
                {
                    book.Borrowed = true;
                }
                if (vet[6].Contains('f'))
                {
                    book.Borrowed = false;
                }
                //Console.WriteLine(book.ToString());
                bookList.Add(book);
            }

            return bookList;
        }

        List<Book> EmprestadosParaLista()
        {
            bookList.Clear();
            StreamReader sr = new("livrosEmprestado.txt");
            string txt = "";
            string aux = "";

            for (int i = 0; sr.EndOfStream == false; i++)
            {
                Book book = new Book();
                Author author = new Author();
                //$"{Title}|{Edition}|{Author}|{Isbn}|{Reading}|{Borrowed}";

                txt = sr.ReadLine();

                //Console.WriteLine(txt);

                string[] vet = txt.Split('|');

                //Console.WriteLine($"{vet[0]}   {vet[1]}   {vet[2]}  {vet[3]}   {vet[4]}  {vet[5]}  {vet[6]}");

                string s = vet[0];
                book.Title = s;

                int n = int.Parse(vet[1]);
                book.Edition = n;

                s = vet[2];
                author.Name = s;
                book.Author = author;

                s = vet[3];
                author.LastName = s;
                book.Author = author;

                s = vet[4];
                book.Isbn = s;

                if (vet[5].Contains('t'))
                {
                    book.Reading = true;
                }
                if (vet[5].Contains('f'))
                {
                    book.Reading = false;
                }

                if (vet[6].Contains('t'))
                {
                    book.Borrowed = true;
                }
                if (vet[6].Contains('f'))
                {
                    book.Borrowed = false;
                }
                //Console.WriteLine(book.ToString());
                bookList.Add(book);
            }

            return bookList;
        }

        List<Book> LeituraParaLista()
        {
            bookList.Clear();
            StreamReader sr = new("livrosLeitura.txt");
            string txt = "";
            string aux = "";

            for (int i = 0; sr.EndOfStream == false; i++)
            {
                Book book = new Book();
                Author author = new Author();
                //$"{Title}|{Edition}|{Author}|{Isbn}|{Reading}|{Borrowed}";

                txt = sr.ReadLine();

                //Console.WriteLine(txt);

                string[] vet = txt.Split('|');

                //Console.WriteLine($"{vet[0]}   {vet[1]}   {vet[2]}  {vet[3]}   {vet[4]}  {vet[5]}  {vet[6]}");

                string s = vet[0];
                book.Title = s;

                int n = int.Parse(vet[1]);
                book.Edition = n;

                s = vet[2];
                author.Name = s;
                book.Author = author;

                s = vet[3];
                author.LastName = s;
                book.Author = author;

                s = vet[4];
                book.Isbn = s;

                if (vet[5].Contains('t'))
                {
                    book.Reading = true;
                }
                if (vet[5].Contains('f'))
                {
                    book.Reading = false;
                }

                if (vet[6].Contains('t'))
                {
                    book.Borrowed = true;
                }
                if (vet[6].Contains('f'))
                {
                    book.Borrowed = false;
                }
                //Console.WriteLine(book.ToString());
                bookList.Add(book);
            }

            return bookList;
        }

        Book FindBook()
        {
            Console.WriteLine("Qual o nome do livro que você quer editar?");
            string s = Console.ReadLine();

            foreach (var item in bookList)
            {
                if (item.Title.Equals(s))
                {
                    return item;
                }
            }
            return null;
        }

        void EditBook(Book book)
        {
            Author author = book.Author;
            string s;
            int n;

            Book booklOld = book;

            /*Console.WriteLine("Qual o novo nome do livro?");
            s = Console.ReadLine();
            book.Title = s;

            Console.WriteLine("Qual a nova edição:");
            n = int.Parse(Console.ReadLine());
            book.Edition = n;

            Console.WriteLine("Qual o novo isbn ?");
            s = Console.ReadLine();
            book.Isbn = s;

            Console.WriteLine("Qual o novo nome do autor?");
            s = Console.ReadLine();
            author.Name = s;

            Console.WriteLine("Qual o novo sobrenome do autor?");
            s = Console.ReadLine();
            author.LastName = s;

            book.Author = author;*/

            Console.WriteLine("Para onde vai o livro: \n[1 - estante]\n[2 - leitura]\n[3 - emprestado]");
            int op = int.Parse(Console.ReadLine());
            if (op == 1)
            {
                book.Reading = false;
                book.Borrowed = false;
            }
            if (op == 2)
            {
                book.Reading = true;
                book.Borrowed = false;
            }
            if (op == 3)
            {
                book.Reading = false;
                book.Borrowed = true;
            }

            string aux = book.ToString();

            //Remover livro do arquivo 
            /*if (booklOld.Borrowed == false && booklOld.Reading == false)
            {
                bookList.Remove(booklOld);
                //LerSepararLista(bookList);
                SobrescreverArquivoEstante(bookList);
            }
            if (booklOld.Borrowed == false && booklOld.Reading == true)
            {
                bookList.Remove(booklOld);
                //LerSepararLista(bookList);
                //SobrescreverArquivoLeitura(bookList);
            }
            if (booklOld.Borrowed == true && booklOld.Reading == false)
            {
                bookList.Remove(booklOld);
                //LerSepararLista(bookList);
                //SobrescreverArquivoEmprestado(bookList);
            }*/

            //Salvar livro no arquivo
            if (book.Borrowed == false && book.Reading == false)
            {
                aux = book.ToString();
                WriteFileEstante(aux);
            }
            if (book.Borrowed == false && book.Reading == true)
            {
                aux = book.ToString();
                WriteFileLeitura(aux);
            }
            if (book.Borrowed == true && book.Reading == false)
            {
                aux = book.ToString();
                WriteFileEmprestado(aux);
            }
            
        }

    }
}