---
id: "035"
order: 35
title: "İçi boş dikdörtgen oluşturma"
level: "İleri"
prerequisites: ["013", "031", "034"]
concepts: ["İç içe döngü", "Sınır hücreleri", "Mantıksal VEYA", "Satır ve sütun konumu", "Tek satır ve sütun"]
---

# İçi boş dikdörtgen oluşturma

## Problem tanımı

Satır ve sütun sayıları verilen bir dikdörtgenin yalnızca kenarlarını yıldızla, iç kısmını boşlukla çizin. Satır sayısı 1 ile 15, sütun sayısı 1 ile 30 arasında olmalıdır. Her çıktı satırı tam sütun sayısı kadar karakter içerir. İlk ve son satır bütünüyle yıldızdır; aradaki satırların yalnızca ilk ve son sütununda yıldız bulunur.

Bir hücrenin kenarda olması için şu dört koşuldan en az biri doğru olmalıdır: ilk satır, son satır, ilk sütun veya son sütun. Bu koşulları mantıksal VEYA ile birleştirin. Satır sayısı 1 ya da sütun sayısı 1 olduğunda bütün hücreler kenardadır; iç boşluk oluşmaz. Özel durumları ayrı çizim algoritmalarıyla değil, aynı sınır koşuluyla ele alın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `satirSayisi` | 1 ile 15 arasında `int`; dikdörtgenin yüksekliği. |
| Girdi | `sutunSayisi` | 1 ile 30 arasında `int`; her satırın genişliği. |
| Çıktı | İçi boş dikdörtgen | Kenar hücreleri `*`, iç hücreleri boşluk; istenen sayıda satır. |

`Satır sayısı (1..15): ` ve `Sütun sayısı (1..30): ` istemlerine sırasıyla iki ayrı satırda tamsayı girin. Satır geçersizse sütun istenmez ve `Hata: Satır sayısı 1 ile 15 arasında bir tamsayı olmalıdır.` yazılır. Sütun geçersizse `Hata: Sütun sayısı 1 ile 30 arasında bir tamsayı olmalıdır.` yazılır. Geçerli çıktıda başlık ve `Sonuç:` öneki bulunmaz.

## Algoritma

1. Satır sayısını okuyup 1 ile 15 aralığında doğrulayın.
2. Sütun sayısını okuyup 1 ile 30 aralığında doğrulayın.
3. Dış döngüde satırı 1'den satır sayısına kadar dolaşın.
4. Her satırda iç döngüyle sütunu 1'den sütun sayısına kadar dolaşın.
5. Hücre ilk veya son satırdaysa ya da ilk veya son sütundaysa bir yıldız yazdırın.
6. Bu dört koşulun hepsi yanlışsa bir boşluk yazdırın.
7. İç döngü bittiğinde satırı bitirin; bütün satırlar bitince programı sonlandırın.

Her hücre tam bir kez değerlendirilir ve tam bir karakter üretir. İç döngü başında mevcut satırın önceki sütunları yazılmıştır; dış döngü başında önceki satırlar tamamlanmıştır. Kenar koşulu doğruysa hücrenin diğer koşullarını ayrıca sınıflandırmaya gerek yoktur. Örneğin bir köşe hem bir satır sınırında hem bir sütun sınırındadır, ancak yine yalnızca bir yıldız yazılır.

3 × 6 dikdörtgende ikinci satırın birinci ve altıncı sütunları kenardır. İkinci ile beşinci sütunlar ise dört koşulu da sağlamaz; dört boşlukla iç kısmı oluşturur. Toplam karakter adedi satır sayısı × sütun sayısıdır; satır sonları bu sayıya katılmaz.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Satır sayısı (1..15): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int satirSayisi) ||
    satirSayisi < 1 || satirSayisi > 15)
{
    Console.WriteLine("Hata: Satır sayısı 1 ile 15 arasında bir tamsayı olmalıdır.");
    return;
}

Console.Write("Sütun sayısı (1..30): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int sutunSayisi) ||
    sutunSayisi < 1 || sutunSayisi > 30)
{
    Console.WriteLine("Hata: Sütun sayısı 1 ile 30 arasında bir tamsayı olmalıdır.");
    return;
}

for (int satir = 1; satir <= satirSayisi; satir++)
{
    for (int sutun = 1; sutun <= sutunSayisi; sutun++)
    {
        bool kenar = satir == 1 || satir == satirSayisi ||
            sutun == 1 || sutun == sutunSayisi;
        if (kenar)
        {
            Console.Write("*");
        }
        else
        {
            Console.Write(" ");
        }
    }
    Console.WriteLine();
}
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. `||`, kenar koşullarından en az biri doğru olduğunda `kenar` değerini doğru yapar. `if / else` her hücrede yalnızca bir karakter yazdırır. Dört ayrı `if` ile yazdırmak köşelerde birden fazla yıldız üretebilirdi. Doğrulamalar çizimden önce tamamlandığı için ikinci girdi geçersiz olduğunda bile kısmi dikdörtgen oluşmaz.

## Örnek çalıştırmalar

Girdiler satır ve sütun sırasındadır. Metin blokları istemler ve kullanıcının yanıtları dışında tam çıktıları gösterir. İç boşluklar korunmalıdır.

1 ve 1 için tek hücre hem satır hem sütun sınırıdır:

```text
*
```

3 ve 6 için:

```text
******
*    *
******
```

4 ve 7 için:

```text
*******
*     *
*     *
*******
```

1 ve 5 için iç satır bulunmaz:

```text
*****
```

4 ve 1 için her hücre ilk sütundadır:

```text
*
*
*
*
```

Üst sınırın tam çıktısı yerine aşağıdaki özellikler denetlenir.

| Girdi | Beklenen özellik |
| --- | --- |
| `15`, `30` | 15 satır ve her satır 30 karakter; 86 yıldız, 364 iç boşluk; ilk ve son satırda 30 yıldız. |
| `2`, `5` | 2 satırın her biri 5 yıldız; iç boşluk yok. |

Geçersiz örneklerde yalnızca ilgili hata satırı yazılır.

| Girdi | Hata çıktısı |
| --- | --- |
| İlk girdi `0` | `Hata: Satır sayısı 1 ile 15 arasında bir tamsayı olmalıdır.` |
| İlk girdi `16` | `Hata: Satır sayısı 1 ile 15 arasında bir tamsayı olmalıdır.` |
| `3`, `0` | `Hata: Sütun sayısı 1 ile 30 arasında bir tamsayı olmalıdır.` |
| `3`, `31` | `Hata: Sütun sayısı 1 ile 30 arasında bir tamsayı olmalıdır.` |
| İlk girdi `2.5`, `üç`, boş satır veya giriş sonu | `Hata: Satır sayısı 1 ile 15 arasında bir tamsayı olmalıdır.` |
| İlk girdi `3`; ikinci girdi `2.5`, `altı`, boş satır veya giriş sonu | `Hata: Sütun sayısı 1 ile 30 arasında bir tamsayı olmalıdır.` |

## Sınır durumları

- Tek satırda bütün hücreler ilk ve son satır koşulunu sağlar; yıldızlar kesintisizdir.
- Tek sütunda bütün hücreler ilk ve son sütun koşulunu sağlar; her satır tek yıldızdır.
- Satır veya sütun sayısı 2 ise iç bölge yoktur; bütün hücreler kenardadır.
- Her iki boyut en az 2 olduğunda yıldız sayısı 2 × satır sayısı + 2 × sütun sayısı - 4'tür. Köşeler bu formülde iki kez sayılmamak için çıkarılır; tek boyutlu durumlarda bu formül doğrudan kullanılmaz.
- Her iki boyut en az 3 olduğunda iç boşluk sayısı (satır sayısı - 2) × (sütun sayısı - 2)'dir. 15 × 30 için 13 × 28 = 364 boşluk ve 450 - 364 = 86 yıldız bulunur.
- Satır sonu her zaman son kenar yıldızından sonra gelir; satır sonunda fazladan boşluk yoktur.
- Negatif, aralık dışı, ondalıklı, metin veya boş giriş ve giriş sonu ilgili alanın doğrulamasında reddedilir.

## Kazanımlar

- Satır ve sütun konumlarından bir hücrenin sınırda olup olmadığını belirleme.
- Dört alternatif kenar koşulunu mantıksal VEYA ile birleştirme.
- Köşelerde birden çok koşul doğru olsa da tek karakter üretme.
- Tek satır ve tek sütun durumlarını genel koşulla doğru çizme.
- Çıktının genişliğini ve kenar karakter adedini bağımsız özelliklerle doğrulama.

## Alıştırmalar

1. Kenarda `*`, iç bölgede boşluk yerine `.` yazdırın. 3 × 6 ve 1 × 6 için yıldız sayısının değişmediğini gösterin.
2. Kenar koşulunu tersine çevirip yalnızca iç bölgeye yıldız, kenarlara boşluk yazdırın. Tek satır ve tek sütun için çıktının nasıl tanımlanacağını açıklayın.
3. Çizim sırasında yıldız ve boşluk adetlerini iki sayaçla bulun. 1 × 1, 4 × 1 ve 15 × 30 için sonuçları formüllerle karşılaştırın.
