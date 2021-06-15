using System;
using System.IO;
using LibMorseCode;

namespace Morse_Code
{
  class Program
    {
    static void Main(string[] args)
        {
         //E:\Morse-Code\SourseString.txt

          string strcode = null,                      //Хранит шифр
          resultStr = null;                           //Хранит результат
          char bufStr = '\0';                         //строка буфер для посимвольного считывания шифра из файла
          bool isMessageExist = false;                //Проверка на то, есть ли сообщение в исходном файле
          int countSpace = 0;                         //Количество пробелов

          Console.WriteLine("Enter path of file with message:");
          string path_source = Console.ReadLine();
          Console.WriteLine("Enter path of result file:");
          string path_result = Console.ReadLine();

          Console.WriteLine("Choose language (russian - r, english - e, translit - t)");
          string language = Console.ReadLine();

          if (language != "e" && language != "r" && language != "t")
          {
              Console.WriteLine("You can choose only r or e or t !!!");
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
                       while (bufStr != ' ') //Считываются символы, пока не получится шифр
                       {
                            bufStr = (char)streamRead.Read();

                             if (bufStr == '\uffff') //Конец файла сообщения
                               break;

                            //Ведем подсчет пробелов для разделения слов в сообщении
                             if (bufStr != ' ') 
                                 countSpace = 0;
                             else
                                 countSpace++;

                             //Запись кода
                              if (bufStr != ' ')
                                 strcode += bufStr;
                       }

                            //Дешифрация кода
                            if (strcode != null)
                                resultStr = Convert.ToString(message.Decode(strcode));

                            //Разделение слов между друг другом
                            if (countSpace == 2)
                                resultStr = Convert.ToString(message.Decode(Convert.ToString(bufStr)));

                            strcode = null;
                            bufStr = '\0';
                  }
                        else if (answer == 'l')
                            //Шифрование символа из сообщения
                            resultStr = message.Code((char)streamRead.Read());
                        else
                            Console.WriteLine("You can choose only c or l!");

                        //Запись результата в файл
                        using (StreamWriter streamWrite = new StreamWriter(path_result, true, System.Text.Encoding.UTF8))
                            streamWrite.Write(resultStr);
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
