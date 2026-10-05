---
id: "074"
order: 74
title: "ref ve dizi parametreleriyle yerinde döndürme"
level: "İleri"
prerequisites: ["043", "044", "071", "073"]
concepts: ["ref parametresi", "Dizi referansı", "Yerinde değiştirme", "Alt aralık sözleşmesi", "Üç ters çevirme"]
---

# ref ve dizi parametreleriyle yerinde döndürme

## Problem tanımı

0..30 elemanlı tamsayı dizisini, verilen negatif olmayan `k` adımı kadar sağa döndürün. Son elemanlar başa geçer; elemanların birbirlerine göre döngüsel sırası korunur. Dizi boşsa sonuç boştur. `k`, 0..2147483647 arasında olabilir; etkin adım dolu dizide `k % n` olarak bulunur.

Ek dizi oluşturmadan üç ters çevirme kullanın: dizinin tamamı, ilk `k % n` elemanı ve kalan bölüm. İşleri `SagaDondur`, `TersCevir` ve `Takas` metotlarına ayırın. `Takas(ref int a, ref int b)` çağıranın iki hücresini değiştirir. Dizi parametresinde `ref` kullanmayın: dizi referansının değer olarak aktarılması mevcut dizinin elemanlarını değiştirmek için yeterlidir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n`, `k` | Ayrı satırlarda 0..30 uzunluk ve negatif olmayan `int` adım. |
| Girdi | Elemanlar | Tam `int` aralığında `n` ayrı satır. |
| Metot | `TersCevir` | Dolu alt aralıkta dahil uçlar geçerli dizindir; boş aralıkta erişim yoktur. |
| Çıktı | Dizi | `Dizi:` ardından değerler; boşsa `Dizi: Boş`. |

İstemler `Eleman sayısı: `, `Döndürme: `, sonra `A[0]: ` ile başlayan istemlerdir. İlk hatalı alanda `Hata: Geçersiz giriş.` yazılır; sonraki alanlar istenmez. EOF, boş satır, ondalıklı giriş ve tür/aralık dışı değer reddedilir. Bütün giriş doğrulanmadan dizi değiştirilmez. Sayısal giriş çevre boşlukları ve işaret kabul eder.

## Algoritma

1. Uzunluğu, döndürmeyi ve bütün elemanları doğrulayın.
2. `SagaDondur` içinde boş dizide hemen dönün; aksi halde etkin adımı kalanla bulun.
3. Etkin adım 0 ise diziyi değiştirmeden dönün.
4. `TersCevir` ile tamamını, sonra ilk etkin adımı, sonra kalan bölümü ters çevirin.
5. `TersCevir`, iki ucu `Takas` ile değiştirip uçları birbirine yaklaştırsın.
6. Ana programda oluşan diziyi yazdırın.

`Takas` iki `ref` parametresine atama yapar; çağrıda da `ref` yazılmalıdır. `TersCevir` içindeki dizin parametreleri değer olarak aktarılır; onları artırıp azaltmak çağıranın değişkenlerini değiştirmez. Üç ters çevirme toplam doğrusal iş yapar ve sabit ek bellek kullanır. Çağıran aynı dizi nesnesini görür.

Dizi referansı değer olarak aktarıldığı için metot içinde parametreye yeni dizi atamak çağıranın dizi değişkenini yeniden bağlamaz. Mevcut nesnenin hücrelerini değiştirmek ise çağırandan görülür. Diziyi yeniden bağlamak için gerekli olabilecek `ref int[]`, bu problemde gerekli değildir. `SagaDondur` metoduna negatif olmayan adım ve `null` olmayan dizi gönderilir; boş dizi kabul edilir.

`1, 2, 3, 4, 5` dizisini iki adım sağa döndürme:

| Metot çağrısı | Sonraki dizi |
| --- | --- |
| Tamamını ters çevir | `5, 4, 3, 2, 1` |
| İlk iki elemanı ters çevir | `4, 5, 3, 2, 1` |
| Kalan üç elemanı ters çevir | `4, 5, 1, 2, 3` |

## C# çözümü

```csharp
using System;
using System.Globalization;

if (!Oku("Eleman sayısı: ", out int n) || n < 0 || n > 30)
{
    Console.WriteLine("Hata: Geçersiz giriş.");
    return;
}
if (!Oku("Döndürme: ", out int k) || k < 0)
{
    Console.WriteLine("Hata: Geçersiz giriş.");
    return;
}
int[] a = new int[n];
for (int i = 0; i < n; i++)
{
    string istem = "A[" + i.ToString(CultureInfo.InvariantCulture) + "]: ";
    if (!Oku(istem, out a[i]))
    {
        Console.WriteLine("Hata: Geçersiz giriş.");
        return;
    }
}

SagaDondur(a, k);
Console.Write("Dizi:");
if (n == 0)
{
    Console.Write(" Boş");
}
for (int i = 0; i < n; i++)
{
    Console.Write(" " + a[i].ToString(CultureInfo.InvariantCulture));
}
Console.WriteLine();

static bool Oku(string istem, out int deger)
{
    Console.Write(istem);
    return int.TryParse(Console.ReadLine(), NumberStyles.Integer,
        CultureInfo.InvariantCulture, out deger);
}

static void SagaDondur(int[] a, int k)
{
    if (a.Length == 0)
    {
        return;
    }
    k %= a.Length;
    if (k == 0)
    {
        return;
    }

    TersCevir(a, 0, a.Length - 1);
    TersCevir(a, 0, k - 1);
    TersCevir(a, k, a.Length - 1);
}

static void TersCevir(int[] a, int sol, int sag)
{
    while (sol < sag)
    {
        Takas(ref a[sol], ref a[sag]);
        sol++;
        sag--;
    }
}

static void Takas(ref int a, ref int b)
{
    int gecici = a;
    a = b;
    b = gecici;
}
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. `void` metot veri döndürmez; bu örnekte sonuç, dizinin değişmiş elemanlarıdır.

## Örnek çalıştırmalar

Önce `n`, sonra `k`, ardından elemanları ayrı satırlarda girin. Bloklar istemlerden sonraki tam sonuçtur.

`n = 5`, `k = 2`, dizi `1, 2, 3, 4, 5`:

```text
Dizi: 4 5 1 2 3
```

`n = 3`, `k = 2147483647`, dizi `7, 8, 9`:

```text
Dizi: 9 7 8
```

`n = 0`, `k = 5`:

```text
Dizi: Boş
```

`n = 3`, `k = 3`, dizi `-1, 0, 1`:

```text
Dizi: -1 0 1
```

## Sınır durumları

- Boş dizi için kalan işlemi yapılmaz; sıfıra bölme önlenir.
- Tek elemanlı dizide her etkin adım 0'dır.
- `k = 0` veya uzunluğun katları diziyi değiştirmez.
- Çok büyük `k`, tekrar tekrar tek adım döndürülmez; önce azaltılır.
- `Takas` aynı hücreyle çağrılırsa değer korunur.
- Negatif döndürme bu arayüzün kabul alanında değildir.

## Kazanımlar

- Değer parametresi, `ref` parametresi ve dizi referansını ayırt etme.
- Büyük işi birbirini çağıran küçük `void` metotlarla kurma.
- Alt aralık uçlarını ve mutasyon etkisini sözleşmede belirtme.
- Boş veri durumunu aritmetik işlemden önce ele alma.
- Geçerli veri üzerinde ek dizi oluşturmadan dönüşüm yapma.

## Alıştırmalar

1. Aynı metotları kullanarak sola döndürme ekleyin; etkin adımı açıklayın.
2. `Takas` çağrılarını sayan bir `ref` sayaç ekleyin; adım 0 durumunu koruyun.
3. Metot içinde `a = new int[a.Length]` ataması yapınca çağıranın neden değişmediğini küçük örnekle gösterin.
