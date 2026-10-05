---
id: "030"
order: 30
title: "Bir sayıyı asal çarpanlarına ayırma"
level: "İleri"
prerequisites: ["021", "027", "028", "029"]
concepts: ["Asal çarpanlara ayırma", "Değişen döngü sınırı", "Tekrarlı bölme", "Aday ilerletme", "Çarpan çokluğu"]
---

# Bir sayıyı asal çarpanlarına ayırma

## Problem tanımı

En az 2 olan bir tamsayıyı asal çarpanlarına ayırın. Çarpanları küçükten büyüğe ve tekrarlarını koruyarak tek satırda yazdırın. Örneğin `12 = 2 * 2 * 3` olduğundan sonuç `Asal çarpanlar: 2 2 3` olmalıdır. Çıktıdaki çarpanların çarpımı başlangıç girdisini vermelidir.

Bir aday kalan sayıyı bölüyorsa bu çarpanı yazdırıp kalanı küçültün; aynı çarpan yeniden bulunabilecekse adayı değiştirmeyin. Bölünmüyorsa sonraki adaya geçin. Kalan küçüldüğü için arama sınırı da değişir. Her adayı ayrıca asallık testinden geçirmek gerekli değildir: daha küçük çarpanlar çıkarıldığı için kalan sayıyı bölen sıradaki aday zaten asaldır.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 2 ile 2147483647 arasında `int`. |
| Çıktı | Asal çarpanlar | `Asal çarpanlar: ` önekiyle, aralarında bir boşluk bulunan artan çarpanlar. |

`Sayı (2..2147483647): ` istemine tek tamsayı girin. Örneğin 36 için çıktı `Asal çarpanlar: 2 2 3 3` olur. Geçersiz girişte `Hata: Sayı 2 ile 2147483647 arasında bir tamsayı olmalıdır.` yazılır. 1, asal çarpanı bulunmadığı için bu çalışmanın giriş aralığına dahil değildir.

## Algoritma

1. Girdiyi okuyup 2 ile 2147483647 aralığını doğrulayın.
2. `kalan` değerini girdiye, `aday` değerini 2'ye eşitleyin.
3. Çıktı öneki olan `Asal çarpanlar:` metnini satırı bitirmeden yazın.
4. `aday <= kalan / aday` olduğu sürece döngüyü sürdürün.
5. Kalan adayın katıysa bir boşlukla beraber adayı yazın ve `kalan /= aday` uygulayın. Aynı adayla tekrar deneyin.
6. Kalan adayın katı değilse adayı bir artırın.
7. Döngü bittiğinde kalan 1'den büyükse bu son asal çarpanı da yazın.
8. Sonuç satırını bitirin.

Her adımda başlangıç sayısı, şimdiye kadar yazılmış çarpanların çarpımı ile `kalan` değerinin çarpımına eşittir. Bölme yolunda kalan en az 2 kat küçülür; diğer yolda aday bir artar. Pozitif kalan büyümez ve aday gerilemez; bu nedenle döngü sonlanır. Koşul yanlış olduğunda kalan bileşik olsaydı kareköküne kadar bir böleni bulunması gerekirdi. Bu aralıktaki tüm adaylar denendiği için 1'den büyük son kalan asaldır.

84 için döngüdeki değişim şöyle izlenir. Son satır döngü sonrası işlemdir.

| Kalan | Aday | İşlem | Yazılan çarpan | Yeni kalan / aday |
| --- | --- | --- | --- | --- |
| 84 | 2 | Bölünür | 2 | 42 / 2 |
| 42 | 2 | Bölünür | 2 | 21 / 2 |
| 21 | 2 | Bölünmez | Yok | 21 / 3 |
| 21 | 3 | Bölünür | 3 | 7 / 3 |
| 7 | 3 | `3 <= 7 / 3` yanlış; son kalanı yaz | 7 | Döngü bitti |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Sayı (2..2147483647): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int n) || n < 2)
{
    Console.WriteLine("Hata: Sayı 2 ile 2147483647 arasında bir tamsayı olmalıdır.");
    return;
}

int kalan = n;
int aday = 2;
Console.Write("Asal çarpanlar:");
while (aday <= kalan / aday)
{
    if (kalan % aday == 0)
    {
        Console.Write(" " + aday.ToString(CultureInfo.InvariantCulture));
        kalan /= aday;
    }
    else
    {
        aday++;
    }
}

if (kalan > 1)
{
    Console.Write(" " + kalan.ToString(CultureInfo.InvariantCulture));
}
Console.WriteLine();
```

Çarpandan önce boşluk yazılması ilk ve sonraki çarpanların aynı biçimde eklenmesini sağlar. Satır sonunda fazladan boşluk bırakılmaz. `kalan /= aday`, `kalan = kalan / aday` işleminin kısa yazımıdır. Adayı her turda artırmak, 8 gibi sayılarda tekrar eden 2 çarpanlarını kaybettirir.

## Örnek çalıştırmalar

İstem metni sonuç sütununa dahil değildir. Çarpanlar tekrarlarıyla gösterilir.

| Girdi | Sonuç | Açıklama |
| --- | --- | --- |
| `2` | `Asal çarpanlar: 2` | En küçük geçerli sayı; gövde çalışmaz. |
| `13` | `Asal çarpanlar: 13` | Asal sayının tek çarpanı kendisidir. |
| `8` | `Asal çarpanlar: 2 2 2` | Aynı adayla tekrar bölme. |
| `36` | `Asal çarpanlar: 2 2 3 3` | İki farklı asalın tekrarları. |
| `84` | `Asal çarpanlar: 2 2 3 7` | Değişen kalan sınırı ve son çarpan. |
| `2147117569` | `Asal çarpanlar: 46337 46337` | Büyük bir asalın karesi; eşitlik sınırı. |
| `2147483646` | `Asal çarpanlar: 2 3 3 7 11 31 151 331` | Üst sınıra yakın bileşik sayı. |
| `2147483647` | `Asal çarpanlar: 2147483647` | En büyük `int` değeri asaldır. |
| `1` | `Hata: Sayı 2 ile 2147483647 arasında bir tamsayı olmalıdır.` | Asal çarpanı olmayan sayı reddedilir. |
| `0` | `Hata: Sayı 2 ile 2147483647 arasında bir tamsayı olmalıdır.` | Alt sınır dışıdır. |
| `2147483648` | `Hata: Sayı 2 ile 2147483647 arasında bir tamsayı olmalıdır.` | `int` sınırı aşılır. |
| `çarpan` | `Hata: Sayı 2 ile 2147483647 arasında bir tamsayı olmalıdır.` | Metin girişi reddedilir. |

## Sınır durumları

- 1 için boş bir çarpan satırı üretilmez; sayı doğrulamada reddedilir.
- Başlangıç girdisi asal ise döngü hiçbir çarpan yazmaz; son kalan başlangıç sayısı olarak yazılır.
- Tam karelerde sınırdaki aday denenmelidir. `<=` yerine `<` kullanılması 4 veya büyük asal karelerinde ayrıştırmayı eksik bırakır.
- Bölünme halinde aday sabit kalır; aynı asalın çokluğu böyle korunur.
- `aday <= kalan / aday` koşulu değişen kalanla yeniden hesaplanır ve taşabilecek `aday * aday` çarpımından kaçınır.
- Bu giriş aralığında son kalan 1'den büyüktür: döngüye giriş koşulu `kalan >= aday * aday` anlamına geldiği için her başarılı bölme sonrasında kalan en az aday kadardır. Son kontrol bu asal kalanın yazdırılmasını sağlar.
- Boş giriş, ondalık sayı, negatif sayı ve `int` dışındaki değerler tek hata satırıyla reddedilir.

## Kazanımlar

- Bir sayının asal çarpanlarını artan sırada ve tekrarlarıyla üretme.
- Bölünebilirliğe göre kalan küçültme veya aday artırma yollarını ayırma.
- 84 için aday, kalan ve yazılan çarpanları adım adım izleme.
- Daha küçük çarpanlar çıkarıldıktan sonra bölünen adayın neden asal olduğunu açıklama.
- Döngü sonunda kalan büyük çarpanın neden asal olduğunu arama sınırıyla gerekçelendirme.
- Tek satırdaki çarpanlar arasında birer boşluk kullanıp sondaki fazladan boşluğu önleme.

## Alıştırmalar

1. Çarpanların toplam sayısını bir sayaçla hesaplayın; 13, 8 ve 36 için sırasıyla 1, 3 ve 4 bulunduğunu doğrulayın.
2. Her çarpanı yazdırırken bir `long` biriktiricide çarpın; işlem sonunda bu çarpımın başlangıç sayısına eşit olduğunu gösterin.
3. Ardışık aynı çarpanları sayarak sonucu `36 = 2^2 * 3^2` biçiminde yazdırın; tek çarpan ve üs 1 durumları için çıktı biçimini tanımlayın.
