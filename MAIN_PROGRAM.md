# Morse-Code
Основная программа осуществления перевода сообщения в азбуку Морзе. Работа программы осуществляется через файлы, поэтому перед запуском стоит записать исходные данные в файл SourseFile.
---------------------------
```
using System;
using System.IO;
using LibMorseCode;

namespace Morse_Code
{
    class Program
    {
        static void Main(string[] args)
        {
            //C:\Users\kurat\source\repos\Разработка ПМ\Morse Code\SourseString.txt

            string stringCode = null, //Хранит шифр
                resultString = null; //Хранит результат
            char bufString = '\0'; //строка буфер для посимвольного считывания шифра из файла
            bool isMessageExist = false; //Проверка на то, есть ли сообщение в исходном файле
            int countProbel = 0; //Количество пробелов

            Console.WriteLine("Enter path of file with message:");
            string path_source = Console.ReadLine();
            Console.WriteLine("Enter path of result file:");
            string path_result = Console.ReadLine();

            Console.WriteLine("Choose language (russian, english, translit): [r/e/t]");
            string language = Console.ReadLine();

            if (language != "e" && language != "r" && language != "t")
            {
                Console.WriteLine("You can choose only r or e or t!");
            }
            else
            {
                if (language == "e") language = "en";
                if (language == "r") language = "rus";
                if (language == "t") language = "translit";

                //Создаем объект для шифровния/дешифрования в зависимости от выбранного языка сообщения
                MorseCode message = new MorseCode(language);
                message.NotifyError += ShowMessage;

                Console.WriteLine("Source string is Code or Letter? [c/l] ");
                char answer = Convert.ToChar(Console.ReadLine());

                //Файл с результатом будем всегда перезаписывать
                using (StreamWriter streamWrite = new StreamWriter(path_result, false, System.Text.Encoding.UTF8)) { } 

                using (StreamReader streamRead = File.OpenText(path_source))
                {
                    while (streamRead.Peek() != -1) //Работаем с исходным файлом, пока не дойдем до его конца
                    {
                        isMessageExist = true;  

                        if (answer == 'c') 
                        {
                            while (bufString != ' ') //Считываем символы, пока не получим шифр
                            {
                                bufString = (char)streamRead.Read();

                                if (bufString == '\uffff') //Конец файла сообщения
                                    break;

                                //Ведем подсчет пробелов для разделения слов в сообщении
                                if (bufString != ' ') 
                                    countProbel = 0;
                                else
                                    countProbel++;

                                //Запись кода
                                if (bufString != ' ')
                                    stringCode += bufString;
                            }

                            //Дешифрация кода
                            if (stringCode != null)
                                resultString = Convert.ToString(message.Decode(stringCode));

                            //Разделение слов между друг другом
                            if (countProbel == 2)
                                resultString = Convert.ToString(message.Decode(Convert.ToString(bufString)));

                            stringCode = null;
                            bufString = '\0';
                        }
                        else if (answer == 'l')
                            //шифрование символа из сообщения
                            resultString = message.Code((char)streamRead.Read());
                        else
                            Console.WriteLine("You can choose only c or l!");

                        //запись результата в файл
                        using (StreamWriter streamWrite = new StreamWriter(path_result, true, System.Text.Encoding.UTF8))
                            streamWrite.Write(resultString);
                    }
                }

                if (isMessageExist == true)
                    Console.WriteLine("Check the result file.");
                else
                    Console.WriteLine("File is empty!");
            }

            Console.ReadLine();
        }
        
        //Функция вывода сообщения об ошибках
        private static void ShowMessage(String textOfError)
        {
            Console.WriteLine(textOfError);
        }
    }
}
```
