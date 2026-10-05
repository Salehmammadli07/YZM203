---
id: "065"
order: 65
title: "Harf frekanslarıyla anagram denetimi"
level: "İleri"
prerequisites: ["049", "061", "064"]
concepts: ["İki veri kümesi", "Frekans farkı", "Tekrarların korunması", "Normalleştirme", "Sıfır denetimi"]
---

# Harf frekanslarıyla anagram denetimi

## Problem tanımı

Yalnız ASCII harfleri ve normal boşluk içeren iki satırın anagram olup olmadığını bulun. Büyük/küçük harf ayrımı yapmayın ve boşlukları yok sayın. Anagram olmak, aynı harflerin aynı adetlerle bulunmasıdır; sıraları farklı olabilir. Örneğin `Dormitory` ile `Dirty room` anagramdır. `aab` ile `abb` aynı farklı harfleri içerir ama adetleri farklı olduğundan anagram değildir.

Her satır 0..200 karakter içerir. Kabul edilen harfler A–Z ve a–z, boşluk U+0020'dir. İki boş satır veya yalnız boşluklardan oluşan iki satır bu kuralla anagramdır. Rakam ve noktalama kabul edilmez. Bir 26 elemanlı fark dizisi kullanın: birinci metin harfleri sayacı artırır, ikinci metin harfleri azaltır. Hazır sıralama, gruplama, harf dönüştürme veya anagram aracı kullanmayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | İki metin | Her biri 0..200 ASCII harf veya normal boşluk; ayrı satırlar. |
| Ara değer | `fark` | Her harfin birinci ve ikinci metindeki adetlerinin farkı. |
| Çıktı | Karar | Tek satır `Anagram: Evet` veya `Anagram: Hayır`. |

İstem sırası `Birinci metin: ` ve `İkinci metin: ` olur. İlk satır hatalıysa ikinci satır istenmez. Herhangi bir satırda EOF, fazla uzunluk, Türkçe harf, rakam, noktalama veya sekme için yalnız `Hata: Metin 0..200 ASCII harf veya boşluk içermelidir.` yazılır. İki giriş de doğru olmadan anagram sonucu üretilmez.

## Algoritma

1. 26 elemanlı fark dizisini sıfırla başlatın.
2. Birinci satırı okuyup tamamını doğrulayın; ikinci satır için aynı doğrulamayı uygulayın.
3. Her doğrulanmış satırda boşlukları atlayıp büyük harfleri küçük harfe dönüştürün.
4. Birinci metinde ilgili harf farkını 1 artırın, ikincide 1 azaltın.
5. Bütün farkları tarayın; ilk sıfır olmayan değerde kararı yanlış yapıp durun.
6. Kararı yazdırın.

Birinci metnin taraması bitince fark dizisi o metnin harf adetleridir. İkinci metnin her harfi kendi adedini düşürür; sonunda bütün hücreler 0 ise her harfin adedi eşittir. Farkların toplamının 0 olması tek başına yetmez: `aab` ve `abb` için a farkı 1, b farkı -1'dir. Her fark -200..200 aralığındadır.

`aab` ve `abb` için:

| Aşama | a farkı | b farkı | Yorum |
| --- | --- | --- | --- |
| Birinci metin bitti | 2 | 1 | Birinci metnin adetleri |
| İkinci metin bitti | 1 | -1 | Aynı uzunluk yeterli değil |
| Son denetim | 1 | -1 | Anagram değil |

## C# çözümü

```csharp
using System;

int[] fark = new int[26];
for (int tur = 0; tur < 2; tur++)
{
    Console.Write(tur == 0 ? "Birinci metin: " : "İkinci metin: ");
    string? metin = Console.ReadLine();
    if (metin is null || metin.Length > 200)
    {
        Console.WriteLine("Hata: Metin 0..200 ASCII harf veya boşluk içermelidir.");
        return;
    }
    for (int i = 0; i < metin.Length; i++)
    {
        char c = metin[i];
        if (!((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' '))
        {
            Console.WriteLine("Hata: Metin 0..200 ASCII harf " +
                "veya boşluk içermelidir.");
            return;
        }
    }
    for (int i = 0; i < metin.Length; i++)
    {
        char c = metin[i];
        if (c == ' ')
        {
            continue;
        }
        if (c >= 'A' && c <= 'Z')
        {
            c = (char)(c + ('a' - 'A'));
        }
        fark[c - 'a'] += tur == 0 ? 1 : -1;
    }
}

bool anagram = true;
for (int i = 0; i < fark.Length; i++)
{
    if (fark[i] != 0)
    {
        anagram = false;
        break;
    }
}

Console.WriteLine(anagram ? "Anagram: Evet" : "Anagram: Hayır");
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. Dış döngü yalnız iki giriş turu yapar; `tur` farkın işaretini belirler. Birinci metnin iç sayaçları ikinci girişten önce hazırlanabilir, ancak sonuç iki giriş doğrulanıp işlendikten sonra yazılır. `continue` mevcut boşluğu atlayıp sonraki karaktere geçer.

## Örnek çalıştırmalar

Her çift iki ayrı giriş satırıdır. Bloklar istemler dışındaki tam sonuçtur.

`Dormitory` ve `Dirty room`:

```text
Anagram: Evet
```

`aab` ve `abb`:

```text
Anagram: Hayır
```

`Ab` ve `b a`:

```text
Anagram: Evet
```

Boş satır ve üç boşluk:

```text
Anagram: Evet
```

`a` ve boş satır:

```text
Anagram: Hayır
```

| Girdi | Beklenen özellik |
| --- | --- |
| Birinci 200 adet A, ikinci 200 adet a | Bütün farklar 0; anagram. |
| Aynı harf adetleri farklı sıralarda | Sıra farkı sonucu değiştirmez. |
| İlk satırda !, 1, ç, sekme, 201 karakter veya EOF | Tek hata; ikinci istem verilmez. |
| İlk satır geçerli, ikinci hatalı veya EOF | Tek hata; anagram sonucu verilmez. |

## Sınır durumları

- Boşluk adetlerinin veya özgün uzunlukların eşit olması gerekmez.
- Aynı farklı harflerin bulunması yeterli değildir; tekrar adetleri korunur.
- Fark dizisinin her hücresi ayrı denetlenir; toplam fark tek karar ölçüsü değildir.
- Harfsiz iki geçerli metin anagramdır.
- Büyük/küçük harf dönüşümü kültüre bağlı araçla yapılmaz.
- İkinci satır hatalıysa birinci satırın sayımı dışarıya sonuç olarak yazılmaz.

## Kazanımlar

- İki veri kümesinin adetlerini tek fark dizisinde karşılaştırma.
- Sıra eşitliği, farklı değer eşitliği ve adet eşitliğini ayırt etme.
- Normalleştirme kuralını iki girdiye aynı biçimde uygulama.
- Toplam değişmezin bütün hücre eşitliğini neden garanti etmediğini örnekleme.
- İç hesaplama ile sonuç üretiminin zamanını ayrı yönetme.

## Alıştırmalar

1. Sıfır olmayan bütün harf farklarını yazdırın; işaretin hangi metinde fazlalık gösterdiğini açıklayın.
2. Harfsiz metinleri ayrı `Harf yok` sonucu yapın; boşluklu anagram örneklerini koruyun.
3. İkinci metne eklenmesi ve ikinci metinden çıkarılması gereken toplam harf adetlerini farklardan hesaplayın.
