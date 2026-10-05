---
id: "071"
order: 71
title: "Metotlarla aralıktaki asal sayıları özetleme"
level: "İleri"
prerequisites: ["027", "037", "060"]
concepts: ["Metot sözleşmesi", "Parametre", "Dönüş değeri", "Yerel değişken", "Yan etkisiz hesaplama"]
---

# Metotlarla aralıktaki asal sayıları özetleme

## Problem tanımı

İki uç dahil olmak üzere verilen aralıktaki asal sayıların adedini, toplamını, ilkini ve sonuncusunu bulun. Uçlar 0..10000 arasında olmalı ve alt uç üst uçtan büyük olmamalıdır. Hiç asal sayı yoksa adet ve toplam 0, ilk ve son asal `Yok` olur.

Asal sayı denetimini `AsalMi(int sayi)` metoduna ayırın. Metot yalnız parametresini inceleyip `bool` döndürmelidir; konsoldan veri okumamalı, sonuç yazmamalı ve dışarıdaki sayaçları değiştirmemelidir. Böylece aynı denetim aralık taramasında ve tek sayı üzerinde kullanılabilir. Önceki asal sayı algoritması korunur; yeni görev, hesaplama ile kullanıcı etkileşiminin sınırlarını belirlemektir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `alt`, `ust` | Ayrı satırlarda 0..10000 tamsayı; `alt <= ust`. |
| Metot | `AsalMi` | Her `int` için asal ise `true`, diğer durumlarda `false`. |
| Çıktı | Özet | `Adet: N`, `Toplam: T`, `İlk asal: X`, `Son asal: Y`. |

İstemler sırayla `Alt uç: ` ve `Üst uç: ` olur. İlk uç geçersizse ikinci uç istenmez. Geçersiz tamsayı, EOF veya aralık ihlalinde yalnız `Hata: Uçlar 0..10000 aralığında ve sıralı olmalıdır.` yazılır. Sayısal girişte çevre boşlukları ve işaret kabul edilir; ondalıklı değer kabul edilmez. Çıktı sayıları kültürden bağımsızdır.

## Algoritma

1. Uçları okuyup doğrulayın; sayaçları ve toplamı 0, ilk/son asalı -1 yapın.
2. Her adayı `AsalMi` metoduna gönderin.
3. Metotta 2'den küçük sayıyı reddedin; 2'den başlayarak olası bölenleri deneyin.
4. Bir bölen bulunursa `false`, bölen bulunmazsa `true` döndürün.
5. Asal adayda adedi ve toplamı artırın; ilk asal henüz yoksa saklayın, son asalı güncelleyin.
6. Dört sonuç satırını yazdırın.

Metodun parametresi değer olarak aktarılır; `sayi`, çağıranın değişkeninden bağımsız yerel değerdir. `return`, bulunduğu metot çağrısını bitirir; aralık taraması sonraki adayla devam eder. Bölen koşulu `bolen <= sayi / bolen`, kare çarpımı oluşturmadan gerekli sınırı sağlar. Toplam `long` tutulur; tarama en fazla 10001 aday içerir.

Metot, üst düzey programın `static` yerel fonksiyonudur. `static`, dış yerel değişkenleri yakalamasını engeller; tek başına yan etkisizlik garantisi değildir. Bu örnekte yan etkisizlik, metotta konsol veya paylaşılan durum kullanılmamasıyla sağlanır. Tanımın çağrıdan sonra yazılması yürütme sırasını değiştirmez. Yeni bir sınıf tanımlamak gerekmez.

2..10 aralığındaki ilk dört aday için:

| Aday | Dönüş | Adet | Toplam |
| --- | --- | --- | --- |
| 2 | `true` | 1 | 2 |
| 3 | `true` | 2 | 5 |
| 4 | `false` | 2 | 5 |
| 5 | `true` | 3 | 10 |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Alt uç: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int alt) || alt < 0 || alt > 10000)
{
    Console.WriteLine("Hata: Uçlar 0..10000 aralığında ve sıralı olmalıdır.");
    return;
}
Console.Write("Üst uç: ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int ust) || ust < alt || ust > 10000)
{
    Console.WriteLine("Hata: Uçlar 0..10000 aralığında ve sıralı olmalıdır.");
    return;
}

int adet = 0, ilk = -1, son = -1;
long toplam = 0;
for (int aday = alt; aday <= ust; aday++)
{
    if (AsalMi(aday))
    {
        adet++;
        toplam += aday;
        if (ilk == -1)
        {
            ilk = aday;
        }
        son = aday;
    }
}

Console.WriteLine("Adet: " + adet.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Toplam: " + toplam.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("İlk asal: " +
    (ilk == -1 ? "Yok" : ilk.ToString(CultureInfo.InvariantCulture)));
Console.WriteLine("Son asal: " +
    (son == -1 ? "Yok" : son.ToString(CultureInfo.InvariantCulture)));

static bool AsalMi(int sayi)
{
    if (sayi < 2)
    {
        return false;
    }

    for (int bolen = 2; bolen <= sayi / bolen; bolen++)
    {
        if (sayi % bolen == 0)
        {
            return false;
        }
    }
    return true;
}
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır.

## Örnek çalıştırmalar

Uçları belirtilen sırayla ayrı satırlarda girin. Bloklar istemlerden sonraki tam sonuçtur.

`alt = 2`, `ust = 10`:

```text
Adet: 4
Toplam: 17
İlk asal: 2
Son asal: 7
```

`alt = 14`, `ust = 16`:

```text
Adet: 0
Toplam: 0
İlk asal: Yok
Son asal: Yok
```

`alt = 17`, `ust = 17`:

```text
Adet: 1
Toplam: 17
İlk asal: 17
Son asal: 17
```

`alt = 10`, `ust = 2`:

```text
Hata: Uçlar 0..10000 aralığında ve sıralı olmalıdır.
```

## Sınır durumları

- 0 ve 1 asal değildir; 0..1 aralığında bütün özet değerleri boş duruma karşılık gelir.
- Eşit uçlar geçerlidir; yalnız bir aday değerlendirilir.
- 10000 üst uçtur ve bileşik sayıdır; döngü onu da dener.
- Metot negatif bir parametre alırsa `false` döndürür; arayüz negatif uç kabul etmez.
- İlk adayın asal olmaması ilk asalın belirlenmesini engellemez.
- Giriş hatasında hiçbir özet satırı yazılmaz.

## Kazanımlar

- Tek bir hesaplamayı parametre ve dönüş değeriyle tanımlama.
- Metot içindeki `return` ile ana programdaki `return` etkisini ayırma.
- Çağıranın sayaçlarını hesaplama metodunun dışında yönetme.
- Metot sözleşmesini arayüzün giriş kısıtından ayrı açıklama.
- Önceki algoritmayı tekrar yazmadan farklı bağlamlarda kullanma.

## Alıştırmalar

1. Aralıktaki asal sayıları da yazdırın; `AsalMi` içine çıktı eklemeyin.
2. İki komşu asal arasındaki en büyük farkı bulun; tek asal durumunu tanımlayın.
3. `AsalMi` metodunu 0, 1, 2, 49 ve 97 ile doğrudan sınayın; beklenen dönüşleri açıklayın.
