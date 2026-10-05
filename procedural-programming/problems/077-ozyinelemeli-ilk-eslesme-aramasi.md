---
id: "077"
order: 77
title: "Özyinelemeli ikili aramayla ilk eşleşmeyi bulma"
level: "İleri"
prerequisites: ["046", "047", "071", "074"]
concepts: ["Özyineleme", "Taban durum", "Küçülen aralık", "İlk eşleşme", "ref çağrı sayacı"]
---

# Özyinelemeli ikili aramayla ilk eşleşmeyi bulma

## Problem tanımı

Küçükten büyüğe, azalmayan sıradaki 0..30 elemanlı bir dizide aranan tamsayının ilk dizinini bulun. Tekrarlı değerlerde soldaki ilk eşleşme seçilir; yoksa -1 yazılır. Diziyi sıralamayın: verilen sırayı doğrulayın, sırasızsa hata üretin.

İkili aramayı döngü yerine kendini çağıran `IlkBul` metoduyla gerçekleştirin. Her çağrı dahil `sol..sag` aralığını inceler ve daha küçük bir aralıkla devam eder. `sol > sag`, hiçbir elemana erişilmeden -1 dönen taban durumdur. Bir `ref` sayaçla bütün çağrıları, boş aralık çağrıları dahil sayın. Sayaç değer karşılaştırması veya çalışma süresi değildir.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n`, dizi | 0..30 uzunluk; tam `int` elemanları, azalmayan sıra. |
| Girdi | Aranan | Tam `int`; sıra doğrulandıktan sonra istenir. |
| Çıktı | Dizin | `Dizin: I`; ilk eşleşme veya -1. |
| Çıktı | Çağrı sayısı | `Çağrı: C`; taban durum dahil her metot girişi. |

İstemler `Eleman sayısı: `, `A[0]: ` ile başlayan istemler, ardından `Aranan: ` olur. İlk hatalı sayısal alanda `Hata: Geçersiz giriş.` yazılır. Sırasız dizide yalnız `Hata: Dizi azalmayan sırada olmalıdır.` yazılır ve aranan değer istenmez. EOF veya boş satır geçersizdir; sayısal giriş çevre boşluklarını ve işareti kabul eder.

## Algoritma

1. Diziyi okuyup `SiraliMi` ile önkoşulu doğrulayın; sonra arananı okuyun.
2. Sayacı 0 yapıp `IlkBul(a, aranan, 0, n - 1, ref cagri)` çağırın.
3. Her çağrı girişinde sayacı artırın; boş aralıkta -1 dönün.
4. Ortayı `sol + (sag - sol) / 2` ile hesaplayın.
5. Orta değer aranan değerden küçükse sağ alt aralığın sonucunu döndürün.
6. Aksi halde sol alt aralıkta ilk eşleşmeyi arayın; varsa onu döndürün.
7. Solda yoksa orta değer eşitse orta dizini, değilse -1 döndürün.

Her alt aralık ortayı dışlar; uzunluk kesin küçülür ve özyineleme sonlanır. Eşit orta değerin solunu aramak ilk eşleşmeyi korur. Her düzey en fazla bir alt çağrı yapar; bütün diziyi dallanarak gezmez. Dizi değiştirilmez; her çağrının `sol`, `sag` ve `orta` değerleri kendine aittir.

Çağrılar yürütme yığınında ayrı yerel durumlar taşır; `ref cagri` ise hepsinin aynı sayaç hücresini artırmasını sağlar. Arama metodunun önkoşulu sıralı dizi ve geçerli bir alt aralıktır; başlangıçta boş dizi için 0..-1 boş aralığı geçerlidir.

`1, 2, 2, 2, 5` dizisinde 2 arama:

| Çağrı | Aralık | Karar |
| --- | --- | --- |
| 1 | 0..4; orta 2 | Eşit; solu araştır. |
| 2 | 0..1; orta 0 | 1 küçüktür; sağa git. |
| 3 | 1..1; orta 1 | Eşit; solu araştır. |
| 4 | 1..0 | Boş; -1 dön. Sonra çağrı 3 dizin 1 döndürür. |

## C# çözümü

```csharp
using System;
using System.Globalization;

if (!Oku("Eleman sayısı: ", out int n) || n < 0 || n > 30)
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
if (!SiraliMi(a))
{
    Console.WriteLine("Hata: Dizi azalmayan sırada olmalıdır.");
    return;
}
if (!Oku("Aranan: ", out int aranan))
{
    Console.WriteLine("Hata: Geçersiz giriş.");
    return;
}

int cagri = 0;
int dizin = IlkBul(a, aranan, 0, n - 1, ref cagri);
Console.WriteLine("Dizin: " + dizin.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("Çağrı: " + cagri.ToString(CultureInfo.InvariantCulture));

static bool Oku(string istem, out int deger)
{
    Console.Write(istem);
    return int.TryParse(Console.ReadLine(), NumberStyles.Integer,
        CultureInfo.InvariantCulture, out deger);
}

static bool SiraliMi(int[] a)
{
    for (int i = 1; i < a.Length; i++)
    {
        if (a[i] < a[i - 1])
        {
            return false;
        }
    }
    return true;
}

static int IlkBul(int[] a, int aranan, int sol, int sag, ref int cagri)
{
    cagri++;
    if (sol > sag)
    {
        return -1;
    }

    int orta = sol + (sag - sol) / 2;
    if (a[orta] < aranan)
    {
        return IlkBul(a, aranan, orta + 1, sag, ref cagri);
    }

    int soldaki = IlkBul(a, aranan, sol, orta - 1, ref cagri);
    if (soldaki != -1)
    {
        return soldaki;
    }
    return a[orta] == aranan ? orta : -1;
}
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. Metot konsola erişmez. Dönen dizin çağrıların dönüşleri boyunca ana programa taşınır.

## Örnek çalıştırmalar

Önce `n`, elemanlar ve arananı ayrı satırlarda girin. Bloklar istemlerden sonraki tam sonuçtur.

`n = 5`, dizi `1, 2, 2, 2, 5`, aranan `2`:

```text
Dizin: 1
Çağrı: 4
```

Aynı dizide aranan `3`:

```text
Dizin: -1
Çağrı: 4
```

`n = 0`, aranan `7`:

```text
Dizin: -1
Çağrı: 1
```

`n = 2`, dizi `2, 1`; aranan istenmez:

```text
Hata: Dizi azalmayan sırada olmalıdır.
```

## Sınır durumları

- Tek elemanda eşleşme olsa bile ilk dizin kuralı için boş sol aralık çağrılır; toplam iki çağrı olur.
- Bütün elemanlar eşitse arama soldaki ilk dizine kadar sürer.
- Aranan küçük veya büyük uç dışında olabilir; -1 normal sonuçtur.
- Boş aralıkta orta dizin hesaplanmaz ve dizi okunmaz.
- `int` uçları karşılaştırılır; farkları alınmaz, sayısal taşma oluşmaz.
- En fazla 30 eleman, özyineleme derinliğini sınırlı tutar; çok büyük problemler için yığın kapasitesi ayrıca düşünülmelidir.

## Kazanımlar

- Özyinelemeli metoda taban durum ve kesin küçülen alt problem koyma.
- Eşitlikte aramayı sürdürerek ilk sonucu koruma.
- Yerel çağrı durumu ile ortak `ref` sayacı ayırma.
- Sıralılık önkoşulunu algoritmadan önce doğrulama.
- Çağrı sayısının hangi işlemi ölçtüğünü açıkça tanımlama.

## Alıştırmalar

1. Son eşleşmeyi bulan sürümü yazın; eşitlikte hangi tarafa gidileceğini değiştirin.
2. Yalnız en yüksek özyineleme derinliğini ölçün; bunu toplam çağrı sayısıyla karşılaştırın.
3. Aynı ilk eşleşmeyi döngülü sürümle bulun; dizin sonuçlarının aynı olduğunu farklı girdilerde denetleyin.
