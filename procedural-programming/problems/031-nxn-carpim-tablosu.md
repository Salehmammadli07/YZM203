---
id: "031"
order: 31
title: "N × N çarpım tablosu oluşturma"
level: "İleri"
prerequisites: ["008", "025"]
concepts: ["İç içe döngü", "Satır ve sütun", "İç sayacı yeniden başlatma", "Sabit genişlik", "Çıktı düzeni"]
---

# N × N çarpım tablosu oluşturma

## Problem tanımı

1 ile 10 arasında verilen N değeri için 1'den N'ye kadar sayıların çarpım tablosunu oluşturun. Tablonun satır ve sütun numaraları 1'den başlar. Her hücrede satır numarası ile sütun numarasının çarpımı bulunur. Örneğin üçüncü satırın ikinci hücresinde 3 × 2 = 6 yazılır.

Çıktı tam N satırdan oluşmalıdır; her satırda N ürün bulunmalıdır. Sayıları sağa yaslanan dört karakterlik alanlarda yazdırın. Tablonun ayrıca başlığı, ayraçları veya `Sonuç:` öneki yoktur. Değerleri bir dizide biriktirmeden, her hücreyi hesaplandığı anda yazdırın. Bu problemde daha önce tek boyutta kullanılan döngü, satır ve sütun için iki ayrı sayaca genişletilir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 1 ile 10 arasında `int`; satır ve sütun sayısı. |
| Ara değer | `satir`, `sutun` | 1 ile N arasında dolaşan iki bağımsız sayaç. |
| Çıktı | Çarpım tablosu | N satır; her satır dört karakterlik N sayı alanı içerir. |

`N (1..10): ` istemine tek tamsayı girin. Geçersiz girişte `Hata: N 1 ile 10 arasında bir tamsayı olmalıdır.` yazılır; tablo üretilmez. Sayının solundaki boşluklar alanın parçasıdır. Satır sonuna ayrıca boşluk eklenmez; son sayı yazıldıktan sonra yalnızca yeni satıra geçilir.

## Algoritma

1. N değerini okuyup 1 ile 10 aralığını doğrulayın.
2. Dış döngüde `satir` sayacını 1'den N'ye kadar ilerletin.
3. Her dış döngü yinelemesinde iç döngünün `sutun` sayacını yeniden 1 yapın.
4. İç döngüde sütunu 1'den N'ye kadar ilerletin; her hücre için `satir * sutun` hesaplayın.
5. Ürünü tamsayı metnine çevirip `PadLeft(4)` ile dört karakterlik alana sağa yaslayarak yazdırın.
6. İç döngü tamamlandığında satırı bitirin; sonraki dış döngü yinelemesine geçin.
7. N. satır tamamlanınca programı bitirin.

Dış döngünün gövdesine her girişte iç `for` ifadesinin başlangıç adımı yeniden çalışır. Önceki satır bittiğinde sütun N + 1'e ulaşmış olsa da yeni satırda tekrar 1 olur. Böylece her satırın ilk ürünü `satir * 1`'dir. İç döngü başında, o satırın 1'den `sutun - 1`'e kadar hücreleri yazılmıştır. Dış döngü başında ise önceki satırlar tamamlanmıştır. N² ürün ve N satır sonu üretilir.

N = 3 için satır geçişi aşağıdaki gibi izlenir. Tablo yalnızca sayaçların görevlerini gösterir; tam çıktı örnekler bölümündedir.

| Adım | Satır | Sütun | İşlem |
| --- | --- | --- | --- |
| İç döngü başlar | 1 | 1 | 1 yazılır. |
| Aynı satır ilerler | 1 | 2, sonra 3 | 2 ve 3 yazılır. |
| İç koşul yanlış olur | 1 | 4 | Yeni satıra geçilir. |
| Dış döngü ilerler | 2 | Yeniden 1 | 2 yazılır; iç sayaç sıfırlanır. |
| Aynı satır ilerler | 2 | 2, sonra 3 | 4 ve 6 yazılır. |
| Son satır başlar | 3 | Yeniden 1 | Sırasıyla 3, 6 ve 9 yazılır. |

Buradaki "sıfırlama", sayacı başlangıç değeri olan 1'e döndürmek anlamındadır; sütunun değeri 0 yapılmaz.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("N (1..10): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int n) || n < 1 || n > 10)
{
    Console.WriteLine("Hata: N 1 ile 10 arasında bir tamsayı olmalıdır.");
    return;
}

for (int satir = 1; satir <= n; satir++)
{
    for (int sutun = 1; sutun <= n; sutun++)
    {
        int urun = satir * sutun;
        Console.Write(urun.ToString(CultureInfo.InvariantCulture).PadLeft(4));
    }
    Console.WriteLine();
}
```

Kod bağımsız bir konsol projesinin `Program.cs` dosyasında çalışır. `Console.Write` satırı bitirmediği için aynı satırın hücreleri yan yana yazılır. Parametresiz `Console.WriteLine` yalnızca iç döngünün dışında olduğundan her tamamlanmış satırda bir kez çalışır. `PadLeft(4)`, hesaplamayı değiştirmez; yalnızca sayı metninin soluna gerekli boşlukları ekler.

## Örnek çalıştırmalar

Aşağıdaki metin blokları, istem ve kullanıcının yanıtı dışındaki tam tablo çıktılarıdır. Soldaki boşluklar korunmalıdır.

N = 1 için tek hücre dört karakterlik alanda yazılır:

```text
   1
```

N = 3 için:

```text
   1   2   3
   2   4   6
   3   6   9
```

N = 4 için iki basamaklı ürünler de sütunlarını korur:

```text
   1   2   3   4
   2   4   6   8
   3   6   9  12
   4   8  12  16
```

Üst sınırın büyük çıktısını aşağıdaki özelliklerle denetleyin. Bu satır tam çıktı gösterimi değildir.

| Girdi | Beklenen özellik |
| --- | --- |
| `10` | 10 satır; her satır 40 karakter; son hücre 100, bu hücrenin alanı ` 100`; toplam 100 ürün. |

Geçersiz örneklerde tablo yerine aşağıdaki tek hata satırı yazılır.

| Girdi | Hata çıktısı |
| --- | --- |
| `0` | `Hata: N 1 ile 10 arasında bir tamsayı olmalıdır.` |
| `11` | `Hata: N 1 ile 10 arasında bir tamsayı olmalıdır.` |
| `-2` | `Hata: N 1 ile 10 arasında bir tamsayı olmalıdır.` |
| `3.5` | `Hata: N 1 ile 10 arasında bir tamsayı olmalıdır.` |
| `üç`, boş satır veya giriş sonu | `Hata: N 1 ile 10 arasında bir tamsayı olmalıdır.` |

## Sınır durumları

- N = 1 için iki döngü de bir kez çalışır; tek ürün ve bir satır sonu oluşur.
- N = 10 için en büyük ürün 100'dür. Üç basamaklı bu sayı dört karakterlik alana sığar; bütün ürünler `int` aralığındadır.
- Sütun her yeni satırda 1'den başlamalıdır. İç sayacı döngülerin dışında bir kez başlatmak sonraki satırların boş kalmasına yol açabilir.
- Yeni satır komutu iç döngünün gövdesinde olursa ürünler tek tek alt alta yazılır; istenen tablo oluşmaz.
- Alan genişliğinin çarpımından her satırın N × 4 karakter olduğu hesaplanabilir. Satır sonu karakterleri bu genişliğe dahil değildir.
- Negatif, sıfır, üst sınır dışı, ondalıklı, metin veya boş giriş ve giriş sonu tek hata satırıyla reddedilir.

## Kazanımlar

- İki boyutlu bir çıktıyı dış satır ve iç sütun döngüleriyle kurma.
- İç döngü sayacının her dış yinelemede neden yeniden başlatıldığını açıklama.
- Hücre hesabını döngü sayaçlarından üretme.
- `Write` ve `WriteLine` konumlarıyla satır düzenini yönetme.
- Sabit genişlikli sayı alanlarıyla farklı basamak sayısındaki değerleri hizalama.

## Alıştırmalar

1. Satır ve sütun sayılarını ayrı iki girdi olarak alın. 2 × 4 ve 4 × 2 tablolarını karşılaştırıp dış ve iç döngü sınırlarını açıklayın.
2. Çarpım yerine `satir + sutun` yazdıran toplama tablosunu oluşturun. N = 3 için bütün hücrelerin beklenen değerlerini önceden hesaplayın.
3. Her satır sonunda o satırın ürünler toplamını yazdırın. Toplam biriktiricisini dış döngünün gövdesinde başlatın ve neden orada olması gerektiğini açıklayın.
