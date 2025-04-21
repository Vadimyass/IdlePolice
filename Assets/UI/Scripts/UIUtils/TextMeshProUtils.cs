using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.Utils;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.UIUtils
{
    public static class TextMeshProUtils
    {
        public static Color GetColorByNumber(float number)
        {
            if (number > 0)
            {
                return Color.green;
            }
            
            return Color.red;
        }

        public static string GetTextForTimer(float timerSeconds)
        {
            var timeSpan = TimeSpan.FromSeconds(timerSeconds);
            var text = string.Empty;
            text += timeSpan.Days > 0 ? timeSpan.Days + "d " : string.Empty;
            text += timeSpan.Hours > 0 ? timeSpan.Hours + "h " : string.Empty;
            text += timeSpan.Minutes > 0 ? timeSpan.Minutes + "m " : string.Empty;
            text += timeSpan.Seconds > 0 ? timeSpan.Seconds + "s " : string.Empty;
            return text;
        }
        
        #region numbers

        private static readonly string[] BasicSuffixes = { "", "K", "M", "B", "T" };

        // Метод для конвертации числа в текст с округлением до тысяч, миллионов и т.д.
        public static string NumberToShortenedText(double number, int digitsAfterPoint = 2)
        {
            if (number < 1000)
                return number.ToString();  // Если меньше тысячи, возвращаем само число

            int suffixIndex = 0;
            double shortenedNumber = number;

            // Округляем до ближайшей величины с суффиксом K, M, B, T
            while (shortenedNumber >= 1000 && suffixIndex < BasicSuffixes.Length - 1)
            {
                shortenedNumber /= 1000d;
                suffixIndex++;
            }

            // Если превысили триллионы, переходим на буквенное обозначение
            if (suffixIndex == BasicSuffixes.Length - 1 && shortenedNumber >= 1000)
            {
                return HandleBeyondTrillions(shortenedNumber, digitsAfterPoint);
            }

            var format = "0.";
            for (int i = 0; i < digitsAfterPoint; i++)
            {
                format += "0";
            }
            
            string numberString = HasDecimalsAfterTwoDigits(shortenedNumber) ? shortenedNumber.ToString(format) : shortenedNumber.ToString("0");

            return numberString + BasicSuffixes[suffixIndex];
        }

        // Метод для работы с числами выше триллиона
        private static string HandleBeyondTrillions(double number, int digitsAfterPoint = 2)
        {
            double integerPart = number;
            int offset = 0;
            char[] alphabet = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

            string suffix = GenerateSuffix(ref integerPart, ref offset, alphabet);
            
            var format = "0.";
            for (int i = 0; i < digitsAfterPoint; i++)
            {
                format += "0";
            }
            
            string numberString = HasDecimalsAfterTwoDigits(integerPart) ? integerPart.ToString(format) : integerPart.ToString("0");
            
            return numberString + suffix;
        }
        
        public static bool HasDecimalsAfterTwoDigits(double number)
        {
            int firstTwoDecimals = (int)((number * 100) % 100);

            // Проверяем, что первые две цифры после запятой не равны нулю
            return firstTwoDecimals != 0;
        }

        // Генерация буквенного суффикса, когда числа превышают триллионы
        private static string GenerateSuffix(ref double integerPart, ref int offset, char[] alphabet)
        {
            string suffix = "";
            while (integerPart >= 1000d)
            {
                integerPart /= 1000d;
                offset++;
            }

            int firstCharIndex = offset-1;

            while (firstCharIndex > 26)
            {
                firstCharIndex -= 26;
            }

            if (offset/26 >= 1 && offset/26 < 2)
            {
                suffix += char.ToUpper(alphabet[firstCharIndex]);
                suffix += alphabet[firstCharIndex];
            }
            else if (offset/26 == 2)
            {
                suffix += char.ToUpper(alphabet[firstCharIndex]);
                suffix += char.ToUpper(alphabet[firstCharIndex]);
            }
            else
            {
                suffix += alphabet[firstCharIndex];
                suffix += alphabet[firstCharIndex];
            }
        
            

            return suffix;
        }

        #endregion
        
        
        public static string ConvertBigDoubleToText(double number, int digitsAfterPoint = 2)
        {
            if (number < 1000)
            {
                return number.ToString("0");
            }
            
            return NumberToShortenedText(number, digitsAfterPoint);
        }
        
        public static string GetFormattedNumberText(double number)
        {
            var formattedNumber = number.ToString("N0");
            return formattedNumber.Replace(',', ' ');
        }
        
        public static async UniTask SetTextGradually(TextMeshProUGUI tmp, string text, float secondsOnOneSymbol = 0.05f)
        {
            tmp.text = String.Empty;
            foreach (var symbol in text)
            {
                if (tmp.text == text)
                {
                    break;
                }
                tmp.text += symbol;
                await UniTask.Delay(TimeSpan.FromSeconds(secondsOnOneSymbol));
            }
        }

        public static string GetStringInFloatFormatOrIntFormat(float number, string format = "0.0")
        {
            return Math.Truncate(number) == number ? number.ToString("0") : number.ToString(format);
        }

        public static async UniTask SetNumberTextUsingRandomNumbers(TextMeshProUGUI tmp, string text, float randomTime)
        {
            tmp.text = String.Empty;
            float currentTime = 0;
            while (randomTime > currentTime)
            {
                tmp.text = Random.Range(10, 99).ToString();
                await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
                currentTime += 0.05f;
            }
            tmp.text = text;
        }

        public static async UniTask DecipherMessageAnimation(TextMeshProUGUI tmp, string text, float secondsOnOneSymbol = 0.05f)
        {
            string cipher = String.Empty;
            foreach (var symbol in text)
            {
                cipher += Random.Range(0, 2);
            }

            tmp.text = cipher;
            for (int i = 0; i < cipher.Length; i++)
            {
                var chars = cipher.ToCharArray();
                chars[i] = text[i];
                for (int j = i+1; j < cipher.Length; j++)
                {
                    chars[j] = Char.Parse(Random.Range(0, 2).ToString());
                }
                cipher = chars.ArrayToString();
                tmp.text = cipher;
                await UniTask.Delay(TimeSpan.FromSeconds(secondsOnOneSymbol));
            }
        }

        public static async UniTask IncreaseNumberFromZero(TextMeshProUGUI tmp, int number, float secondsForIncrease = 2f, string addSymbol = "")
        {
            float visibleScore = 0;
            DOTween.To(() => visibleScore, x =>
            {
                visibleScore = x;
                tmp.text = addSymbol + visibleScore.ToString("0.");
            }, number, secondsForIncrease).SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(secondsForIncrease));
        }
        
        public static async UniTask DecreaseNumberToZero(TextMeshProUGUI tmp, int number, float secondsForIncrease = 2f)
        {
            float visibleScore = number;
            DOTween.To(() => visibleScore, x =>
            {
                visibleScore = x;
                tmp.text = visibleScore.ToString("0.");
            }, 0, secondsForIncrease).SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(secondsForIncrease));
        }
        
        public static async UniTask DecreaseNumberToZeroTimeFormat(TextMeshProUGUI tmp, int number, float secondsForIncrease = 2f)
        {
            float visibleScore = number * 60;
            DOTween.To(() => visibleScore, x =>
            {
                visibleScore = x;
                var hours = visibleScore / 60;
                var minutes = visibleScore % 60;
                tmp.text = hours.ToString("00") + ":" + minutes.ToString("00");
            }, 0, secondsForIncrease).SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(secondsForIncrease));
        }
        
        public static async UniTask ChangeNumberToAnother(TextMeshProUGUI tmp, int startNumber, int endNumber, float secondsForIncrease = 2f)
        {
            DOTween.To(() => startNumber, x =>
            {
                startNumber = x;
                tmp.text = startNumber.ToString("0.");
            }, endNumber, secondsForIncrease).SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(secondsForIncrease));
        }
    }
}