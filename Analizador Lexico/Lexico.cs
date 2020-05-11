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
    public partial class Lexico : Form
    {
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
        public Lexico()
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
                var palabras = this.espacio_de_texto.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int inicio = 0;
               
                
                if (tk.Name == "ERROR"){
                    dgvTokens.Rows.Add(tk.Name, tk.Lexema, tk.Index, tk.Linea);
                    //espacio_de_texto.ForeColor = System.Drawing.Color.Red;
                    foreach (var item in palabras)
                    {
                        if (item == tk.Lexema)
                        {
                            inicio = this.espacio_de_texto.Text.IndexOf(item, tk.Index);
                            this.espacio_de_texto.Select(inicio, item.Length);
                            this.espacio_de_texto.SelectionColor = Color.Red;
                            this.espacio_de_texto.SelectionStart = this.espacio_de_texto.Text.Length;
                            inicio++;
                        }
                    }
                }
                else
                {
                    if(tk.Name == "COMENTARIO")
                    {
                        dgvTokens.Rows.Add(tk.Name, tk.Lexema, tk.Index, tk.Linea);
                        foreach (var item in palabras)
                        {
                            if (item == tk.Lexema)
                            {
                                inicio = this.espacio_de_texto.Text.IndexOf(item, tk.Index);
                                this.espacio_de_texto.Select(inicio, item.Length);
                                this.espacio_de_texto.SelectionColor = Color.LightGray;
                                this.espacio_de_texto.SelectionStart = this.espacio_de_texto.Text.Length;
                                inicio++;
                            }
                        }
                    }
                    else
                    {
                        if (tk.Name != "ESPACIO")
                        {
                            if (tk.Name == "IDENTIFICADOR")
                                if (reservadas.Contains(tk.Lexema))
                                    tk.Name = "RESERVADO";
                            foreach (var item in palabras)
                            {
                                if (item == tk.Lexema)
                                {
                                    inicio = this.espacio_de_texto.Text.IndexOf(item, tk.Index);
                                    this.espacio_de_texto.Select(inicio, item.Length);
                                    this.espacio_de_texto.SelectionColor = Color.Lime;
                                    this.espacio_de_texto.SelectionStart = this.espacio_de_texto.Text.Length;
                                    inicio++;
                                }
                            }
                            dgvTokens.Rows.Add(tk.Name, tk.Lexema, tk.Index, tk.Linea);
                        }
                    }
                    
                }
            }
        }

        private void espacio_de_texto_TextChanged_1(object sender, EventArgs e)
        {
            analizarCodigo();

        }

        private void button1_Click(object sender, EventArgs e)
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
                        while ((texto = sr.ReadLine()) != null)
                        {
                            espacio_de_texto.Text += texto+"\n";
                        }
                        sr.Close();
                    }
                }
            }
        }
    }
}
