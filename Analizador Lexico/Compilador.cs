using Analizador_Lexico.Sintactico;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analizador_Lexico
{
    public partial class Compilador : Form
    {
        Timer timer = new Timer();
        private void Compilador_Load(object sender, EventArgs e)
        {
            timer.Interval = 10;
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
        String[] reservadas = {
         "if", "else",
        "for", "while",
        "return", "case",
        "var", "main",
        "console", "class",
        "function", "throw",
        "let", "const",
        "get", "set",
        "new", "break",
        "continue", "in",
        "switch", "try",
        "document", "write",
        "catch", "do",
        "delete", "this",
        "try", "default",
        "with", "String",
        "int", "double",
        "bool", "float",
         };
        List<Token> tokens = new List<Token>();
        List<String> token_names = new List<String>();
        Regex rex;
        StringBuilder pattern = null;
        int[] group_numbers;
        public Compilador()
        {
            InitializeComponent();
            //se añaden todos los tokens con sus patrones
            tokens.Add(new Token(@"\d*\.?\d+", "DIGITO", false));
            tokens.Add(new Token(@"'\\.'|'[^\\]'", "CARACTER", false));
            tokens.Add(new Token("\".*?\"", "CDENA", false));
            tokens.Add(new Token(@"\s+", "ESPACIO", false));
            tokens.Add(new Token(@"//[^\r\n]*", "COMENTARIO", false));
            tokens.Add(new Token(@">|<|==|>=|<=|!", "COMPARADOR", false));
            tokens.Add(new Token(@"[\(\)\{\}\[\];,]", "DELIMITADOR", false));
            tokens.Add(new Token(@"[\.\+\-/*%]", "OPERADOR", false));
            tokens.Add(new Token(@"\b[_a-zA-Z][\w]*\b", "IDENTIFICADOR", false));
            tokens.Add(new Token(@"&&|\|\|", "OPERADOR_LOGICO", false));
            tokens.Add(new Token(@"\$", "OPERADOR_DE_CONCATENACIÓN", false));
            tokens.Add(new Token(@"-=|\+=|\*=|\/=|%=|=|:", "OPERADOR_DE_ASIGNACIÓN", false));
            tokens.Add(new Token(@"\+\+", "OPERADOR_DE_INCREMENTO", false));
            tokens.Add(new Token(@"--", "OPERADOR_DE_DECREMENTO", false));

            //Se recorren los tokens que se agregaron junto con sus patrones
            foreach (Token token in tokens)
            {
                if (pattern == null)
                    //el nombre de el token y el patron se agrupan al patron principal mediante la agrupacion de expresiones regulares
                    pattern = new StringBuilder(string.Format("(?<{0}>{1})", token.getToken_name(), token.getPatron()));

                if (!token.getIgnore())
                {
                    //si el token no es ignorado
                    //el nombre de el token y el patron se agrupan al patron principal mediante la agrupacion de expresiones regulares
                    pattern.Append(string.Format("|(?<{0}>{1})", token.getToken_name(), token.getPatron()));
                    token_names.Add(token.getToken_name());
                }
            }
            //despues de crear el patron final con todos los tokens names y sus patrones se crea la expresion regular
            rex = new Regex(pattern.ToString(), RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture | RegexOptions.Multiline);

            //se inicia una lista de nombres de los tokens, en este caso se le da el tamaño de los tokens encontrados
            group_numbers = new int[token_names.Count];
            //Se crea un arreglo que contiene los nombres de todos los grupos de los patrones
            string[] gn = rex.GetGroupNames();

            //se calculan los numeros de cada nombre de token
            for (int i = 0, idx = 0; i < gn.Length; i++)
            {
                if (token_names.Contains(gn[i]))
                {
                    group_numbers[idx++] = rex.GroupNumberFromName(gn[i]);
                }
            }
        }

        public IEnumerable<Tokens> GetTokens(string text)
        {
            //Se hace el match con el texto y la expresion regular
            Match match = rex.Match(text);

            if (!match.Success) yield break;

            //variables para tener un conteo de lineas y index
            int line = 1, start = 0, index = 0;

            //si se encuentra una coincidencia con el patron
            while (match.Success)
            {
                //si el index es mayor al index actual
                if (match.Index > index)
                {

                    string token = text.Substring(index, match.Index - index);

                    yield return new Tokens("ERROR", token, index, line);
                    //se recalculan las lineas
                    line += contarLineas(token, index, ref start);
                }

                //se recorre el arreglo de numeros de los tokens
                for (int i = 0; i < group_numbers.Length; i++)
                {
                    //si se encuentra una coincidencia con el grupo de patrones con numero
                    if (match.Groups[group_numbers[i]].Success)
                    {

                        string name = rex.GroupNameFromNumber(group_numbers[i]);
                        //se añade el el token a la lista de tokens
                        yield return new Tokens(name, match.Value, match.Index, line);

                        break;
                    }
                }
                //se recalculan las lineas
                line += contarLineas(match.Value, match.Index, ref start);
                //se recalcula el index
                index = match.Index + match.Length;
                //y se pasa al siguiente match
                match = match.NextMatch();
            }


            if (text.Length > index)
            {
                //si el tamaño de el texto es mayor a el index actual
                yield return new Tokens("ERROR", text.Substring(index), index, line);
            }
        }
        private int contarLineas(string token, int index, ref int line_start)
        {
            int line = 0;

            for (int i = 0; i < token.Length; i++)
                if (token[i] == '\n')
                {
                    line++;
                    line_start = index + i + 1;
                }

            return line;
        }
        private void analizarCodigo()
        {
            dgvTokens.Rows.Clear();

            foreach (var tk in this.GetTokens(espacio_de_texto.Text))
            {                
                if (tk.Name == "ERROR"){
                    dgvTokens.Rows.Add(tk.Name, tk.Lexema, tk.Index, tk.Linea);
                }
                else
                {
                    if(tk.Name == "COMENTARIO")
                    {
                        dgvTokens.Rows.Add(tk.Name, tk.Lexema, tk.Index, tk.Linea);
                    }
                    else
                    {
                        if (tk.Name != "ESPACIO")
                        {
                            if (tk.Name == "IDENTIFICADOR")
                                if (reservadas.Contains(tk.Lexema))
                                    tk.Name = "RESERVADO";
                            dgvTokens.Rows.Add(tk.Name, tk.Lexema, tk.Index, tk.Linea);
                        }
                    }
                    
                }
            }
        }

        private void espacio_de_texto_TextChanged_1(object sender, EventArgs e)
        {
            analizarCodigo();
            analisisSintactico();
            pictureBox1.Refresh();

        }
        private void analisisSintactico()
        {
            Gramatica gramatica = new Gramatica();
            LanguageData languaje = new LanguageData(gramatica);
            Parser parser = new Parser(languaje);
            ParseTree parseTree = parser.Parse(espacio_de_texto.Text);
            ParseTreeNode node = parseTree.Root;

            if (node == null)
            {
                results.ForeColor = Color.Red;
                results.Text = ">>>>>";
                espacio_de_texto.ForeColor = Color.Red;

                for (int i = 0; i < parseTree.ParserMessages.Count; i++)
                {
                    results.Text +=parseTree.ParserMessages[i].Message + "\n>>>>>linea: " + parseTree.ParserMessages[i].Location.Line + "\n";
                }
            }
            else
            {
                results.ForeColor = Color.Lime;
                espacio_de_texto.ForeColor = Color.Lime;
                results.Text = ">>>>>Correcto";
                var arbol = new ParseTreeClass(parseTree);
                var nodes = arbol.Traverse();

                results.Text += ("\nArbol:\n" + arbol);
            }
        }

        private void abrirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    String path = openFileDialog.FileName;
                    filePath.Text = path;
                    using (StreamReader sr = new StreamReader(@path))
                    {
                        string texto;
                        espacio_de_texto.Text = "";
                        while ((texto = sr.ReadLine()) != null)
                        {
                            espacio_de_texto.Text += texto + "\n";
                        }
                        sr.Close();
                    }
                }
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog fileDialog = new SaveFileDialog())
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    String name = fileDialog.FileName;
                    StreamWriter textoAGuardar = File.CreateText(name);
                    textoAGuardar.Write(espacio_de_texto.Text);
                    textoAGuardar.Flush();
                    textoAGuardar.Close();
                    filePath.Text = name;
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int caracter = 0;
            int altura = espacio_de_texto.GetPositionFromCharIndex(0).Y;
            if (espacio_de_texto.Lines.Length > 0)
            {
                for (int i = 0; i < espacio_de_texto.Lines.Length - 1; i++)
                {
                    e.Graphics.DrawString((i + 1).ToString(), espacio_de_texto.Font, Brushes.Orange, pictureBox1.Width - e.Graphics.MeasureString((i + 1).ToString(), espacio_de_texto.Font).Width + 2, altura);
                    caracter += espacio_de_texto.Lines[i].Length + 1;
                    altura = espacio_de_texto.GetPositionFromCharIndex(caracter).Y;
                }
            }
            else
            {
                e.Graphics.DrawString("1", espacio_de_texto.Font, Brushes.Orange, pictureBox1.Width - (e.Graphics.MeasureString("1", espacio_de_texto.Font).Width + 10), altura);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
