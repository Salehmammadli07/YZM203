---
id: "079"
order: 79
title: "Geri izlemeyle hedef toplamlı alt kümeleri sayma"
level: "İleri"
prerequisites: ["040", "045", "075", "077", "078"]
concepts: ["Geri izleme", "Seç veya dışla", "Budama", "Kalan toplam", "Değerle aktarılan arama durumu"]
---

# Geri izlemeyle hedef toplamlı alt kümeleri sayma

## Problem tanımı

0..16 elemanlı, her değeri 1..20 olan dizide toplamı verilen hedefe eşit alt kümelerin sayısını bulun. Hedef 0..320 arasında olabilir. Bir eleman en fazla bir kez seçilir. Aynı değere sahip farklı dizinler ayrı seçimlerdir: `1, 1` dizisinde hedef 1 için iki alt küme vardır. Hedef 0 için tek çözüm boş alt kümedir.

Her konumda elemanı seçme ve dışlama dalları oluşturan özyinelemeli arama kullanın. Toplam hedefi aştıysa veya kalan bütün elemanları eklemek bile hedefe ulaşamıyorsa dalı budayın. `kalan[i]`, `i` konumundan sona kadar toplamdır. Pozitif değer kısıtı bu budamaları güvenli yapar. Hazır alt küme üretimi veya dinamik programlama kullanmayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n`, hedef | Ayrı satırlarda 0..16 uzunluk ve 0..320 hedef. |
| Girdi | Elemanlar | Ayrı satırlarda 1..20 pozitif tamsayı. |
| Çıktı | Çözüm adedi | `Alt küme: N`; dizin seçimleri ayrı sayılır. |
| Çıktı | Arama ölçüsü | `Ziyaret: Z`; budanmış ve başarılı çağrılar dahil her giriş. |

İstemler `Eleman sayısı: `, `Hedef: `, ardından `A[0]: ` ile başlayan istemlerdir. İlk geçersiz alanda yalnız `Hata: Geçersiz giriş.` yazılır; diğer alanlar istenmez. EOF, boş satır veya aralık ihlali geçersizdir. Sayısal giriş çevre boşluklarını ve işareti kabul eder. Tam veri alınmadan arama başlamaz.

## Algoritma

1. Uzunluk, hedef ve pozitif elemanları doğrulayın.
2. Uzunluğu `n + 1` olan kalan toplam dizisini sondan başa kurun; `kalan[n] = 0` olsun.
3. `Say` metodunu konum 0 ve toplam 0 ile çağırın; her girişte ziyaret sayısını artırın.
4. Toplam hedefe eşitse 1 dönün: kalan pozitif elemanların hepsi dışlanmalıdır.
5. Dizi bittiyse, toplam fazlaysa veya toplam ile kalan toplam hedefe yetmiyorsa 0 dönün.
6. Elemanı seçen ve dışlayan iki çağrının çözüm sayılarını toplayıp döndürün.
7. Çözüm adedini ve ziyaret sayısını yazdırın.

İki dal birbirinden ayrıdır ve her dizin için seçim bir kez yapılır. Konum her çağrıda artar; en fazla `n + 1` düzey vardır. Durum değer parametreleriyle taşındığından seçimden dönerken ortak bir toplamı geri almak gerekmez. Bu, geri izlemeyi yerel çağrı durumuyla kurar. En fazla 65536 seçim ve 131071 çağrı bulunur; budama genellikle daha azını inceler. Sayılar `int` aralığına sığar.

Arama metodunun önkoşulları pozitif dizi, onunla uyumlu kalan toplam dizisi ve geçerli konumdur. Başlangıç toplamı 0'dır. İki dal ayrı yerel `toplam` değerleri alır; yalnız ziyaret sayacı ortaktır. Çözüm listesi tutulmaz.

`1, 1`, hedef 1 için bütün çağrılar:

| Ziyaret | Konum ve toplam | Sonuç |
| --- | --- | --- |
| 1 | 0, 0 | İlk elemanı seçme ve dışlama dalları. |
| 2 | 1, 1 | Başarılı; 1 çözüm. |
| 3 | 1, 0 | İkinci eleman için iki dal. |
| 4 | 2, 1 | Başarılı; 1 çözüm. |
| 5 | 2, 0 | Dizi bitti; 0 çözüm. |

## C# çözümü

```csharp
using System;
using System.Globalization;

if (!Oku("Eleman sayısı: ", out int n) || n < 0 || n > 16)
{
    Console.WriteLine("Hata: Geçersiz giriş.");
    return;
}
if (!Oku("Hedef: ", out int hedef) || hedef < 0 || hedef > 320)
{
    Console.WriteLine("Hata: Geçersiz giriş.");
    return;
}
int[] a = new int[n];
for (int i = 0; i < n; i++)
{
    string istem = "A[" + i.ToString(CultureInfo.InvariantCulture) + "]: ";
    if (!Oku(istem, out a[i]) || a[i] < 1 || a[i] > 20)
    {
        Console.WriteLine("Hata: Geçersiz giriş.");
        return;
    }
}

int[] kalan = new int[n + 1];
for (int i = n - 1; i >= 0; i--)
{
    kalan[i] = a[i] + kalan[i + 1];
}
int ziyaret = 0;
int adet = Say(a, kalan, 0, 0, hedef, ref ziyaret);
Console.WriteLine("Alt küme: " + adet.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Ziyaret: " + ziyaret.ToString(CultureInfo.InvariantCulture));

static bool Oku(string istem, out int deger)
{
    Console.Write(istem);
    return int.TryParse(Console.ReadLine(), NumberStyles.Integer,
        CultureInfo.InvariantCulture, out deger);
}

static int Say(int[] a, int[] kalan,
    int i, int toplam, int hedef, ref int ziyaret)
{
    ziyaret++;
    if (toplam == hedef)
    {
        return 1;
    }
    if (i == a.Length || toplam > hedef || toplam + kalan[i] < hedef)
    {
        return 0;
    }

    int sec = Say(a, kalan, i + 1, toplam + a[i], hedef, ref ziyaret);
    int disla = Say(a, kalan, i + 1, toplam, hedef, ref ziyaret);
    return sec + disla;
}
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. Genel iş miktarı üstel olabilir; 16 eleman sınırı ve budama, bu öğretim örneğinin kapsamını belirler.

## Örnek çalıştırmalar

Önce `n`, hedef ve elemanları ayrı satırlarda girin. Bloklar istemlerden sonraki tam sonuçtur.

`n = 3`, hedef `3`, dizi `1, 2, 3`:

```text
Alt küme: 2
Ziyaret: 13
```

`n = 2`, hedef `1`, dizi `1, 1`:

```text
Alt küme: 2
Ziyaret: 5
```

`n = 2`, hedef `7`, dizi `2, 4`:

```text
Alt küme: 0
Ziyaret: 1
```

`n = 0`, hedef `0`:

```text
Alt küme: 1
Ziyaret: 1
```

## Sınır durumları

- Boş dizi ve pozitif hedef için 0 çözüm, bir ziyaret vardır.
- Hedef 0 her pozitif dizide boş seçimin tek çözümüdür; ilk çağrıda dönülür.
- Eşit elemanlar ayrı dizinlerdeyse ayrı alt kümeler üretir; tekrarlar silinmez.
- Bütün elemanların toplamı hedeften küçükse kökte budama yapılır.
- Sıfır veya negatif eleman bu sürümde kabul edilmez; erken başarı ve budama gerekçesini değiştirir.
- On altı adet 1 için hedef 8, 12870 ayrı dizin seçimi üretir.

## Kazanımlar

- Seçim problemini birbirini dışlayan iki dala ayırma.
- Bir budamanın dayandığı veri önkoşulunu açıklama.
- Dönüş değerleriyle çözüm sayılarını birleştirme.
- Değer parametreleriyle dalların durumlarını bağımsız tutma.
- Üstel aramanın giriş boyutu ve ölçüm sınırlarını tanımlama.

## Alıştırmalar

1. Budamaları kaldırıp aynı çözüm sayısını ve ziyaret farkını ölçün.
2. Seçilmiş dizinleri tamponda tutup çözümleri de yazdırın; geri dönüşte geçerli seçili adedini koruyun.
3. Sıfır değerleri kabul etmek için erken başarı kuralını değiştirin; hedef 0 için birden fazla çözümü gösterin.
