using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analizador_Lexico.Sintactico
{
    class Gramatica : Grammar
    {
        public Gramatica() : base(caseSensitive: true)
        {
            #region comentarios
            CommentTerminal comentario = new CommentTerminal("<comentario>","<-","\n","\r\n");
            CommentTerminal bloqueComentario = new CommentTerminal("<bloqueComentario>", "<!-", "->");
            base.NonGrammarTerminals.Add(bloqueComentario);
            base.NonGrammarTerminals.Add(comentario);
            #endregion

            #region Expresiones regulares
            RegexBasedTerminal numero = new RegexBasedTerminal(@"\d*\.?\d+");
            RegexBasedTerminal identificador = new RegexBasedTerminal(@"\b[_a-zA-Z][\w]*\b");
            RegexBasedTerminal caracter = new RegexBasedTerminal(@"'\\.'|'[^\\]'");
            RegexBasedTerminal cadenaDeTexto = new RegexBasedTerminal("\".*?\"");
            #endregion

            #region No Terminales
            NonTerminal inicio = new NonTerminal("INICIO");
            NonTerminal importacionDeLibreria = new NonTerminal("IMPORTACIÓN_DE_LIBRERIA");
            NonTerminal bloqueDeCodigo = new NonTerminal("BLOQUE_DE_CODIGO");
            NonTerminal funcionMain = new NonTerminal("FUNCTION_MAIN");
            NonTerminal declaracionDeVariable = new NonTerminal("DECLARACIÓN_DE_VARIABLR");
            NonTerminal tipoDeDato = new NonTerminal("TIPO_DE_DATO");
            NonTerminal variable = new NonTerminal("VARIABLE");
            NonTerminal condicion = new NonTerminal("CONDICIÓN");
            NonTerminal dato = new NonTerminal("DATO");
            NonTerminal bloqueFunc = new NonTerminal("BLOQUE_DE_FUNCIÓN");
            NonTerminal bloqueIf = new NonTerminal("BLOQUE_IF");
            NonTerminal bloqueReturn = new NonTerminal("BLOQUE_RETURN");
            NonTerminal parametros = new NonTerminal("PARAMETROS");
            NonTerminal concatenacion = new NonTerminal("CONCATENACIÓN");
            NonTerminal bloqueFor = new NonTerminal("BLOQUE_FOR");
            NonTerminal condicionFor = new NonTerminal("CONDICIÓN_FOR");
            NonTerminal bloqueElse = new NonTerminal("BLOQUE_ELSE");
            NonTerminal incrementoVariable = new NonTerminal("INCREMENTO_DE_VARIABLE");
            NonTerminal imprimirEnTerminal = new NonTerminal("IMPRIMIR_EN_CONSOLA");
            NonTerminal operacionAritmetica = new NonTerminal("OPERACIÓN_ARITMETICA");
            NonTerminal operacionDeAsignacion = new NonTerminal("OPERACIÓN_DE_ASIGNACIÓN");
            NonTerminal operadoresDeComparacion = new NonTerminal("OPERADORES_DE_COMPARACIÓN");
            NonTerminal asignacionDeValor = new NonTerminal("ASIGNACIÓN_DE_VALOR");
            #endregion

            #region Terminales

            #region Palabras Reservadas
            MarkReservedWords(
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
                    "bool", "float");

            KeyTerm var = ToTerm("var");
            KeyTerm let = ToTerm("let");
            KeyTerm function = ToTerm("function");
            KeyTerm console = ToTerm("console");
            KeyTerm log = ToTerm("log");
            KeyTerm for_ = ToTerm("for");
            KeyTerm while_ = ToTerm("while");
            KeyTerm if_ = ToTerm("if");
            KeyTerm else_ = ToTerm("else");
            KeyTerm do_ = ToTerm("do");
            KeyTerm const_ = ToTerm("const");
            KeyTerm return_ = ToTerm("return");
            KeyTerm switch_ = ToTerm("switch");
            KeyTerm case_ = ToTerm("case");
            KeyTerm default_ = ToTerm("default");
            KeyTerm new_ = ToTerm("new");
            KeyTerm set_ = ToTerm("set");
            KeyTerm get = ToTerm("get");
            KeyTerm import_ = ToTerm("import");
            KeyTerm in_ = ToTerm("in");
            KeyTerm delete = ToTerm("delete");
            KeyTerm continue_ = ToTerm("continue");
            #endregion

            #region operadores aritmeticos
            KeyTerm suma = ToTerm("+");
            KeyTerm resta = ToTerm("-");
            KeyTerm multiplicacion = ToTerm("*");
            KeyTerm division = ToTerm("/");
            KeyTerm modulo = ToTerm("%");
            #endregion

            #region operadores Lógicos
            KeyTerm and = ToTerm("&&");
            KeyTerm or = ToTerm("||");
            #endregion

            #region operadores de Asignación
            KeyTerm igual = ToTerm("=");
            KeyTerm igualIncremento = ToTerm("+=");
            KeyTerm igualDecremento = ToTerm("-=");
            #endregion

            #region operadores de comparación
            KeyTerm igualIgual = ToTerm("==");
            KeyTerm diferente = ToTerm("!");
            KeyTerm diferenteIgual = ToTerm("!=");
            KeyTerm mayor = ToTerm(">");
            KeyTerm menor = ToTerm("<");
            KeyTerm mayorIgual = ToTerm(">=");
            KeyTerm menorIgual = ToTerm("<=");
            #endregion

            #region operadores de incremento
            KeyTerm incrementoMas = ToTerm("++");
            KeyTerm incrementoMenos = ToTerm("--");
            #endregion

            #region simbolos
            KeyTerm simboloConcatenacion = ToTerm("$");
            KeyTerm punto = ToTerm(".");
            KeyTerm coma = ToTerm(",");
            KeyTerm puntoYComa = ToTerm(";");
            KeyTerm llaveAbierta = ToTerm("{");
            KeyTerm llaveCerrada = ToTerm("}");
            KeyTerm corcheteAbierto = ToTerm("[");
            KeyTerm corcheteCerrado = ToTerm("]");
            KeyTerm parentesisAbierto = ToTerm("(");
            KeyTerm parentesisCerrado = ToTerm(")");
            #endregion

            #endregion

            #region Reglas de producción
            inicio.Rule = importacionDeLibreria + funcionMain |
                                                  funcionMain |
                                         importacionDeLibreria |
                                         bloqueDeCodigo | bloqueDeCodigo + funcionMain |
                                         importacionDeLibreria + bloqueDeCodigo |
                                         importacionDeLibreria + bloqueDeCodigo + funcionMain;

            funcionMain.Rule = function + parentesisAbierto + parentesisCerrado +
               llaveAbierta + bloqueDeCodigo + llaveCerrada |
               function + parentesisAbierto + parentesisCerrado + llaveAbierta + llaveCerrada;

            funcionMain.ErrorRule = SyntaxError + "}";

            importacionDeLibreria.Rule = import_ + cadenaDeTexto;


            bloqueDeCodigo.Rule = bloqueFunc | bloqueFunc + bloqueDeCodigo |
                             declaracionDeVariable | declaracionDeVariable + bloqueDeCodigo |
                                imprimirEnTerminal | imprimirEnTerminal + bloqueDeCodigo |
                                          bloqueIf | bloqueIf + bloqueDeCodigo |
                                         bloqueFor | bloqueFor + bloqueDeCodigo |
                                          bloqueReturn;
            bloqueDeCodigo.ErrorRule = SyntaxError + "}";

            bloqueFor.Rule = for_ + parentesisAbierto + asignacionDeValor + puntoYComa + condicionFor + puntoYComa + incrementoVariable + parentesisCerrado +llaveAbierta +
                bloqueDeCodigo + llaveCerrada | for_ + parentesisAbierto + asignacionDeValor + puntoYComa + condicionFor + puntoYComa + incrementoVariable + parentesisCerrado + llaveAbierta +
                 llaveCerrada;

            condicionFor.Rule = dato + operadoresDeComparacion + dato | dato + operadoresDeComparacion + numero;

            incrementoVariable.Rule = identificador + incrementoMas | identificador + incrementoMenos;

            bloqueReturn.Rule = return_ + identificador;


            bloqueFunc.Rule = function + identificador + parentesisAbierto + parametros + parentesisCerrado + llaveAbierta + bloqueDeCodigo + llaveCerrada |
                function + identificador + parentesisAbierto + parametros + parentesisCerrado + llaveAbierta + llaveCerrada |
                function + identificador + parentesisAbierto + parentesisCerrado + llaveAbierta + bloqueDeCodigo + llaveCerrada |
                function + identificador + parentesisAbierto + parentesisCerrado + llaveAbierta + llaveCerrada;

            parametros.Rule = variable | variable + coma + parametros;

            bloqueIf.Rule = if_ + parentesisAbierto + condicion + parentesisCerrado + llaveAbierta + bloqueDeCodigo + llaveCerrada + bloqueElse |
                if_ + parentesisAbierto + condicion + parentesisCerrado + llaveAbierta + bloqueDeCodigo + llaveCerrada |
                           if_ + parentesisAbierto + condicion + parentesisCerrado + llaveAbierta + llaveCerrada + bloqueElse |
                           if_ + parentesisAbierto + condicion + parentesisCerrado + llaveAbierta + llaveCerrada |
                              if_ + parentesisAbierto + condicion + parentesisCerrado + llaveAbierta
                               + bloqueDeCodigo + llaveCerrada + bloqueIf;


            condicion.Rule = dato + operadoresDeComparacion + dato |
                                                       diferente + dato |
                             diferente + operadoresDeComparacion + dato |
                                                condicion + and + condicion |
                                                condicion + or + condicion;

            operadoresDeComparacion.Rule = igualIgual |
                                                mayor |
                                           mayorIgual |
                                                menor |
                                           menorIgual |
                                        diferenteIgual;

            bloqueElse.Rule = else_ + llaveAbierta + bloqueDeCodigo + llaveCerrada | else_ + llaveAbierta + llaveCerrada;

            declaracionDeVariable.Rule = tipoDeDato + variable;

            operacionDeAsignacion.Rule = igual | resta | suma |division | multiplicacion | modulo;

            operacionAritmetica.Rule = identificador + multiplicacion + identificador | identificador + multiplicacion + numero |
                                                identificador + resta + identificador | identificador + resta + numero |
                                             identificador + division + identificador | identificador + division + numero |
                                               identificador + modulo + identificador | identificador + modulo + numero |
                                                  identificador + suma + identificador | identificador + suma + numero;

            dato.Rule = numero |
                 cadenaDeTexto |
                 identificador |
                       caracter;

            concatenacion.Rule = simboloConcatenacion + dato | simboloConcatenacion + parentesisAbierto + dato + parentesisCerrado |
                simboloConcatenacion + dato | simboloConcatenacion + operacionAritmetica |
                simboloConcatenacion + parentesisAbierto + operacionAritmetica + parentesisCerrado |
                simboloConcatenacion + dato + concatenacion | simboloConcatenacion + parentesisAbierto + operacionAritmetica + parentesisCerrado + coma + concatenacion |
                simboloConcatenacion + dato + concatenacion | simboloConcatenacion + parentesisAbierto + dato + parentesisCerrado + concatenacion;

            imprimirEnTerminal.Rule = console + punto + log + parentesisAbierto + dato + parentesisCerrado |
                                      console + punto + log + parentesisAbierto + dato + concatenacion + parentesisCerrado;

            variable.Rule = identificador | identificador + coma + variable | asignacionDeValor | asignacionDeValor + coma + variable;

            asignacionDeValor.Rule = identificador + igual + dato | identificador + igual + identificador + operacionDeAsignacion + dato;

            tipoDeDato.Rule = var |
                               let;


            #endregion

            this.Root = inicio;
        }
    }
}
