---
id: "061"
order: 61
title: "Karakter sınıflandırma ve harf frekansı"
level: "İleri"
prerequisites: ["048", "059", "060"]
concepts: ["String ve char", "Karakter aralığı", "Harf normalleştirme", "Frekans dizisi", "Eşit sıklık kuralı"]
---

# Karakter sınıflandırma ve harf frekansı

## Problem tanımı

En fazla 200 karakterlik tek bir satırı inceleyin. Harf, rakam, normal boşluk ve diğer karakter adetlerini ayrı sayın. A–Z harflerini a–z ile aynı kabul ederek 26 elemanlı frekans dizisi oluşturun. En sık harfi ve adedini bulun; eşitlikte alfabetik olarak küçük harfi seçin. Hiç harf yoksa bunu ayrı sonuçla belirtin. Örneğin `aA bB!2` içinde dört harf vardır; a ve b ikişer kez geçer, seçilen harf a'dır.

Bu problemde veri alanı yazdırılabilir ASCII karakterleri, yani 32..126 kodlarıdır. Harfler yalnız A–Z ve a–z, rakamlar yalnız 0–9'dur; boşluk U+0020'dir. Türkçe harfler, emoji, sekme ve diğer karakterler giriş alanının dışındadır. Bu sınır sayesinde her kabul edilen karakter C# içinde bir `char` ve bir dizin konumudur. Boş satır geçerlidir; girişin sonu (EOF) boş satır değildir. Hazır karakter sınıflandırma, harf dönüştürme veya gruplama araçları kullanmayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `metin` | 0..200 yazdırılabilir ASCII karakteri; tek satır. |
| Ara değer | `frekans` | 26 sayaç; a dizini 0, z dizini 25. |
| Çıktı | Sınıflar | `Harf: H`, `Rakam: R`, `Boşluk: B`, `Diğer: D`. |
| Çıktı | En sık harf | `En sık harf: c` ve `Frekans: F`; harf yoksa `En sık harf: Yok`. |

`Metin: ` istemini bir satırla yanıtlayın. Bütün karakterler doğrulanmadan sayım veya sonuç üretimi başlamaz. EOF, 201 veya daha fazla karakter ve kabul edilmeyen herhangi bir karakter için yalnız `Hata: Metin 0..200 yazdırılabilir ASCII karakteri içermelidir.` yazılır.

## Algoritma

1. Satırı okuyun; null değerini ve uzunluk sınırını denetleyin.
2. Her karakterin kodunun 32..126 aralığında olduğunu doğrulayın.
3. Dört sınıf sayacını ve 26 harf sayacını 0 yapın.
4. Büyük harfi kod farkıyla küçük harfe dönüştürün; harfi, rakamı, boşluğu veya diğer sınıfını sayın.
5. Harf için `frekans[c - 'a']` sayacını artırın.
6. Frekans dizisini a'dan z'ye tarayın; yalnız kesin büyük adette en sık harfi güncelleyin.
7. Sınıfları ve harf bulunma durumuna uygun sonucu yazdırın.

Her kabul edilen karakter tam bir sınıfa girer; dört sayacın toplamı metnin uzunluğudur. Harf frekanslarının toplamı harf adedidir. Artan alfabetik dizinlerde yalnız büyük adette güncellemek eşitlikte ilk harfi korur. Tarama sonlu uzunlukla sınırlıdır; bütün sayaçlar en fazla 200 olur.

Karakter ile karakter kodu farklı değerlerdir. `Console.Write('A')` harfi, `Console.Write((int)'A')` ise 65 sayısını yazar. Harfi frekans dizinine dönüştürürken kullanılan `c - 'a'` ifadesi de karakter üretmez; a'ya göre sayısal konumu hesaplar.

`aA bB!2` için birikimli sayım:

| Okunan bölüm | Harf | Rakam | Boşluk | Diğer |
| --- | --- | --- | --- | --- |
| aA | 2 | 0 | 0 | 0 |
| aA bB | 4 | 0 | 1 | 0 |
| aA bB!2 | 4 | 1 | 1 | 1 |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Metin: ");
string? metin = Console.ReadLine();
if (metin is null || metin.Length > 200)
{
    Console.WriteLine("Hata: Metin 0..200 yazdırılabilir ASCII " +
        "karakteri içermelidir.");
    return;
}
for (int i = 0; i < metin.Length; i++)
{
    if (metin[i] < ' ' || metin[i] > '~')
    {
        Console.WriteLine("Hata: Metin 0..200 yazdırılabilir ASCII " +
            "karakteri içermelidir.");
        return;
    }
}

int harf = 0, rakam = 0, bosluk = 0, diger = 0;
int[] frekans = new int[26];
for (int i = 0; i < metin.Length; i++)
{
    char c = metin[i];
    if (c >= 'A' && c <= 'Z')
    {
        c = (char)(c + ('a' - 'A'));
    }
    if (c >= 'a' && c <= 'z')
    {
        harf++;
        frekans[c - 'a']++;
    }
    else if (c >= '0' && c <= '9')
    {
        rakam++;
    }
    else if (c == ' ')
    {
        bosluk++;
    }
    else
    {
        diger++;
    }
}

int enSikDizin = -1, enSikAdet = 0;
for (int i = 0; i < frekans.Length; i++)
{
    if (frekans[i] > enSikAdet)
    {
        enSikAdet = frekans[i];
        enSikDizin = i;
    }
}

Console.WriteLine("Harf: " + harf.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Rakam: " + rakam.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Boşluk: " + bosluk.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Diğer: " + diger.ToString(CultureInfo.InvariantCulture));
if (enSikDizin == -1)
{
    Console.WriteLine("En sık harf: Yok");
}
else
{
    Console.WriteLine("En sık harf: " + (char)('a' + enSikDizin));
    Console.WriteLine("Frekans: " +
        enSikAdet.ToString(CultureInfo.InvariantCulture));
}
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. `string` değiştirilemez; yerel `c` karakterini değiştirmek özgün metni değiştirmez. Karakterler çıkarıldığında sonuç sayısal kod farkıdır. `-1` yalnız harf dizini için bulunamadı işaretidir; gerçek dizinler 0..25'tir. Sayısal çıktı biçimi kültürden bağımsızdır.

## Örnek çalıştırmalar

Bloklar `Metin: ` isteminden sonraki tam sonuçtur. Boş satır örneğinde Enter'a basılır.

`aA bB!2`:

```text
Harf: 4
Rakam: 1
Boşluk: 1
Diğer: 1
En sık harf: a
Frekans: 2
```

`ZzZ 09?`:

```text
Harf: 3
Rakam: 2
Boşluk: 1
Diğer: 1
En sık harf: z
Frekans: 3
```

Boş satır:

```text
Harf: 0
Rakam: 0
Boşluk: 0
Diğer: 0
En sık harf: Yok
```

`123!`:

```text
Harf: 0
Rakam: 3
Boşluk: 0
Diğer: 1
En sık harf: Yok
```

| Girdi | Beklenen özellik |
| --- | --- |
| 200 adet A | Harf adedi ve a frekansı 200; diğer sınıflar 0. |
| ASCII 32..126 karakterlerinin her biri bir kez | Harf 52, rakam 10, boşluk 1, diğer 32; seçilen harf a, frekans 2. |
| ç, emoji, sekme, 201 karakter veya EOF | Yalnız tanımlanan hata satırı; sınıf veya harf sonucu yok. |

## Sınır durumları

- Boş satırın uzunluğu 0'dır; hiçbir sayım döngüsü çalışmaz.
- A ile a aynı harf sayacına gider; özgün metin korunur.
- Rakam, noktalama ve boşluk harf yarışına katılmaz.
- Hiç harf yoksa frekans satırı yazılmaz.
- Eşit pozitif frekanslarda alfabetik küçük harf seçilir.
- C# `char` genel olarak UTF-16 kod birimidir; bu problemde ASCII sınırı çok birimli karakterleri dışlar.
- Hatalı karakter satırın sonunda olsa da önceki bölümün sayımı yazılmaz.

## Kazanımlar

- `string` diziniyle karakter okuyup `char` kod aralıklarını karşılaştırma.
- Harf değerini 26 elemanlı frekans dizisinin dizinine dönüştürme.
- Normalleştirme ile özgün veriyi korumayı birlikte uygulama.
- Sınıf adetleri ve toplam uzunluk arasındaki değişmezi doğrulama.
- Harf olmayan veri ile hatalı karakteri problem kuralına göre ayırma.

## Alıştırmalar

1. Pozitif frekanslı bütün harfleri alfabetik sırayla yazdırın; frekans toplamını harf adediyle karşılaştırın.
2. Eşit sıklıkta alfabetik büyük harfi seçin; güncelleme koşulunu ve harfsiz durumunu açıklayın.
3. Türkçe alfabeyi açık küçük/büyük harf eşleme dizileriyle destekleyen sürümü tasarlayın; I, İ, ı ve i eşlemesini örnekleyin.
