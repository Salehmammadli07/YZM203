---
id: "069"
order: 69
title: "Harf ve adet kodunu doğrulayarak açma"
level: "İleri"
prerequisites: ["062", "067", "068"]
concepts: ["Harf-adet dilbilgisi", "Değişken uzunluklu alan", "İşlem öncesi sınır", "Toplam kapasite", "Kısmi çıktıyı önleme"]
---

# Harf ve adet kodunu doğrulayarak açma

## Problem tanımı

067'deki harf-adet biçimini açın. Kod, bir ASCII harfi ve hemen ardından pozitif ondalık adetten oluşan çiftlerin art arda yazılmasıdır. Örneğin `A3b2A1` sonucu `AAAbbA` olur. Adet en az 1, en fazla 200'dür ve başında sıfır olamaz. Kod satırı 0..400 karakter, açılmış metin en fazla 200 karakter olmalıdır.

Boş kod geçerlidir ve boş metin üretir. Rakamlar yalnız 0–9, harfler yalnız A–Z ve a–z'dir; boşluk, işaret veya başka karakter kabul edilmez. Aynı harfin ardışık çiftlerde bulunması geçerlidir: `A1A1` sonucu `AA` olur; kodun en kısa biçimde yazılması şart değildir. Çok basamaklı adedi elle ayrıştırın. `Parse`, `TryParse`, düzenli ifade, hazır tekrar veya açma aracı kullanmayın.

Girişin sonundaki hata daha önce açılmış bölümü çıktı yapmamalıdır. Bu nedenle sonucu 200 karakterlik tamponda hazırlayın; ancak bütün çiftler doğru ve toplam kapasite yeterliyse yazdırın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `kod` | 0..400 karakter; tanımlanan harf-adet çiftleri. |
| Ara değer | `adet`, `yazma` | Mevcut grubun adedi ve toplam üretilmiş karakter sayısı. |
| Çıktı | Açılmış metin | `Metin:` ardından dikey çubuklar arasında sonuç; çubuklar metne dahil değildir. |
| Çıktı | Uzunluk | `Uzunluk: L`; ayraçlar L'ye dahil değildir. |

`Kod: ` isteminde yalnız kodu girin; 067 çıktısındaki başlık ve çubuklar giriş değildir. EOF, uzunluk ihlali, yanlış çift biçimi, sıfır/başında sıfırlı adet, 200'den büyük adet veya toplamın 200'ü aşması için yalnız `Hata: Kod geçersiz veya açılmış metin 200 karakteri aşıyor.` yazılır.

## Algoritma

1. Kod satırını okuyup 0..400 uzunluğunu doğrulayın; 200 karakterlik tampon ayırın.
2. Kod dizinini ve yazma konumunu 0 yapın.
3. Sıradaki karakteri ASCII harfi olarak doğrulayıp saklayın; dizini ilerletin.
4. Sonraki karakterin 1..9 rakamı olduğunu denetleyin; eksik veya başında sıfırlı adedi reddedin.
5. Ardışık rakamları tüketin; her basamak öncesinde `adet <= (200 - basamak) / 10` sınırını denetleyip adedi biriktirin.
6. Adet kalan tampon kapasitesinden büyükse hata verin; aksi halde harfi adet kadar tampona yazın.
7. Sonraki harf-adet çiftini işleyin; bütün kod bitince geçerli tampon bölümünü ve uzunluğu yazdırın.

Her çift sonrası tampon, o ana kadar doğrulanmış kodun tam açılımıdır. Adet sınırı ve toplam kapasite sınırı farklı denetimlerdir: iki ayrı 150'lik grup tek başlarına geçerli ama birlikte fazla uzundur. Tamponun hiçbir yazımı 200. dizine ulaşmaz. Kod dizini harfi ve bütün rakamlarını tüketir; sonraki karakter bir sonraki çiftin başlangıcıdır.

`A3b2A1` için:

| Çift | Adet | Yazma konumu sonrası | Tamponun geçerli bölümü |
| --- | --- | --- | --- |
| A3 | 3 | 3 | AAA |
| b2 | 2 | 5 | AAAbb |
| A1 | 1 | 6 | AAAbbA |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Kod: ");
string? kod = Console.ReadLine();
if (kod is null || kod.Length > 400)
{
    Console.WriteLine("Hata: Kod geçersiz veya açılmış metin " +
        "200 karakteri aşıyor.");
    return;
}

char[] tampon = new char[200];
int dizin = 0, yazma = 0;
while (dizin < kod.Length)
{
    char harf = kod[dizin];
    if (!((harf >= 'A' && harf <= 'Z') || (harf >= 'a' && harf <= 'z')))
    {
        Console.WriteLine("Hata: Kod geçersiz veya açılmış metin " +
            "200 karakteri aşıyor.");
        return;
    }
    dizin++;
    if (dizin == kod.Length || kod[dizin] < '1' || kod[dizin] > '9')
    {
        Console.WriteLine("Hata: Kod geçersiz veya açılmış metin " +
            "200 karakteri aşıyor.");
        return;
    }

    int adet = 0;
    while (dizin < kod.Length && kod[dizin] >= '0' && kod[dizin] <= '9')
    {
        int basamak = kod[dizin] - '0';
        if (adet > (200 - basamak) / 10)
        {
            Console.WriteLine("Hata: Kod geçersiz veya açılmış metin " +
                "200 karakteri aşıyor.");
            return;
        }
        adet = adet * 10 + basamak;
        dizin++;
    }

    if (adet > tampon.Length - yazma)
    {
        Console.WriteLine("Hata: Kod geçersiz veya açılmış metin " +
            "200 karakteri aşıyor.");
        return;
    }
    for (int i = 0; i < adet; i++)
    {
        tampon[yazma++] = harf;
    }
}

string sonuc = new string(tampon, 0, yazma);
Console.WriteLine("Metin: |" + sonuc + "|");
Console.WriteLine("Uzunluk: " + yazma.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. `dizin == kod.Length || ...` kısa devre koşulu eksik adette dizi dışı erişimi önler. Başlangıç rakamı 1..9 kuralı `A0` ve `A01` biçimlerini reddeder; sonraki rakamlar 0 olabilir. Tampon, sonradan hata görülse bile yalnız bellekte kalır; başarılı sonuç ancak dış döngü bittikten sonra yazılır.

## Örnek çalıştırmalar

Bloklar `Kod: ` isteminden sonraki tam sonuçtur.

`A3b2A1`:

```text
Metin: |AAAbbA|
Uzunluk: 6
```

`a1b1a1b1`:

```text
Metin: |abab|
Uzunluk: 4
```

`A1A1`:

```text
Metin: |AA|
Uzunluk: 2
```

Boş satır:

```text
Metin: ||
Uzunluk: 0
```

`A0`, `A01`, `A`, `3A`, `A201`, `A200B1` veya EOF:

```text
Hata: Kod geçersiz veya açılmış metin 200 karakteri aşıyor.
```

| Girdi | Beklenen özellik |
| --- | --- |
| z200 | 200 adet z; uzunluk 200. |
| A100b100 | İki grup, toplam 200; geçerli. |
| a1 çiftinin 200 tekrarı | Kod uzunluğu 400, sonuç 200 adet a; geçerli. |
| A150b150 | Her adet geçerli; toplam uzunluk nedeniyle hata. |
| A2b, A2!, A2b01 | Sonraki çift hatalı; AA şeklinde kısmi çıktı yok. |
| A ardından 399 adet 9; 401 karakter; sekme veya Türkçe harf | Hata; büyük sayıyı biriktirirken taşma yapılmaz. |

## Sınır durumları

- Boş kod tek geçerli harfsiz girdidir; EOF değildir.
- Her harfin ardından en az bir rakam gerekir.
- Adet 0 ve başında sıfırlı biçimler geçersizdir; adet 10 ve 100 geçerlidir.
- Harfin büyüklüğü açılmış metinde korunur.
- Aynı harfin komşu çiftlerde bulunması hata değildir.
- Grup adedi uygun olsa da toplam kapasite aşılabilir.
- 067 ile kodlanıp bu algoritmayla açılan kabul edilmiş metin aynen geri gelir.
- Her geçerli kodu açıp yeniden kodlamak özgün kodu vermeyebilir; A1A1 yeniden A2 olur.

## Kazanımlar

- Uzunlukları değişen harf ve sayı alanlarını ortak dizinle ayrıştırma.
- Alan sınırı ile toplam çıktı kapasitesini ayrı doğrulama.
- Çarpmadan önce sınır denetimini daha küçük bir sayı alanına uyarlama.
- Kısmi çalışma sonucunu kullanıcıya yazmadan hata yönetme.
- Kodlama ve açma ilişkisini iki yönün farklı özellikleriyle açıklama.

## Alıştırmalar

1. Hatalı çiftin başlangıç dizinini hata mesajına ekleyin; çok basamaklı adetlerde bu dizini koruyun.
2. Aynı harfin komşu çiftlerde bulunmasını reddeden sürüm yazın; A1A1 ile A1a1 davranışını ayırın.
3. Önce yalnız doğrulayıp toplam uzunluğu hesaplayan, sonra tam uzunlukta tampon ayırarak açan iki geçişli sürüm hazırlayın.
