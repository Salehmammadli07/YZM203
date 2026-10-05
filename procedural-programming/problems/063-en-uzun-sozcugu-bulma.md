---
id: "063"
order: 63
title: "En uzun sözcüğü ve başlangıç dizinini bulma"
level: "İleri"
prerequisites: ["041", "062"]
concepts: ["Sözcük sınırı", "Başlangıç ve uzunluk", "İlk eşit aday", "İki aşamalı tarama", "Dizinlerin korunması"]
---

# En uzun sözcüğü ve başlangıç dizinini bulma

## Problem tanımı

Yalnız A–Z, a–z ve normal boşluk U+0020 içeren, en fazla 200 karakterlik satırın en uzun sözcüğünü bulun. Sözcük, kesintisiz en uzun harf bölümüdür. Eşit uzunlukta birden çok sözcük varsa girişte ilk görüneni seçin. Sözcüğü, özgün satırdaki sıfır tabanlı başlangıç dizinini ve uzunluğunu yazdırın. Örneğin `  bir uzun  iki` için sonuç `uzun`, başlangıç dizini 6 ve uzunluk 4'tür.

Boş satır ve yalnız boşluklardan oluşan satır geçerlidir; sözcük bulunamadığını bildirin. Başlangıç dizini bulunmadan metni normalleştirmeyin; boşlukları kaldırmak özgün konumları değiştirir. Harf büyüklüğü korunur. `Split`, `Trim`, `Substring`, hazır en büyük değer veya sözcük bulma araçları kullanmayın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `metin` | 0..200 ASCII harf veya normal boşluk. |
| Ara değer | `baslangic`, `uzunluk` | Mevcut sözcüğün özgün konumu ve eleman adedi. |
| Çıktı | Bulunan sözcük | `Sözcük:` ardından dikey çubuklar arasında sözcük; `Başlangıç dizini: I`, `Uzunluk: L`. |
| Çıktı | Sözcük yok | Tek satır `Sonuç: Sözcük yok`. |

`Metin: ` istemini bir satırla yanıtlayın. EOF, uzunluk üst sınırının aşılması, rakam, noktalama, Türkçe harf veya sekme için yalnız `Hata: Metin 0..200 ASCII harf veya boşluk içermelidir.` yazılır. Satırın tamamı taramadan önce doğrulanır.

## Algoritma

1. Satırı, uzunluğunu ve bütün karakterlerini doğrulayın.
2. Tarama dizinini 0, en iyi başlangıcı -1 ve en iyi uzunluğu 0 yapın.
3. Dizin sınır içinde ve karakter boşlukken ilerleyin.
4. Sözcüğün başlangıcını mevcut dizin olarak saklayın; boşluk gelene veya giriş bitene kadar ilerleyin.
5. Dizin ile başlangıcın farkından sözcük uzunluğunu hesaplayın.
6. Uzunluk mevcut en iyiden kesin büyükse başlangıcı ve uzunluğu güncelleyin; taramayı sürdürün.
7. Hiç sözcük yoksa bunu bildirin; aksi halde seçili karakterleri ve özgün konumu yazdırın.

Tarama dizini geri gitmez. Her dış turda ya bir boşluk bölümü ya bir sözcük tüketilir; sonunda son boşluklar da giriş sonuna ulaşır. En iyi aday yalnız kesin uzunluk artışında değiştiği için eşitlikte ilk sözcük kalır. Seçili bölüm özgün satırda `[enIyiBaslangic, enIyiBaslangic + enIyiUzunluk)` aralığıdır.

`  bir uzun  iki` için:

| Sözcük | Başlangıç | Uzunluk | Kayıtlı aday |
| --- | --- | --- | --- |
| bir | 2 | 3 | bir |
| uzun | 6 | 4 | uzun |
| iki | 12 | 3 | uzun |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Metin: ");
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
        Console.WriteLine("Hata: Metin 0..200 ASCII harf veya boşluk içermelidir.");
        return;
    }
}

int dizin = 0, enIyiBaslangic = -1, enIyiUzunluk = 0;
while (dizin < metin.Length)
{
    while (dizin < metin.Length && metin[dizin] == ' ')
    {
        dizin++;
    }
    int baslangic = dizin;
    while (dizin < metin.Length && metin[dizin] != ' ')
    {
        dizin++;
    }
    int uzunluk = dizin - baslangic;
    if (uzunluk > enIyiUzunluk)
    {
        enIyiBaslangic = baslangic;
        enIyiUzunluk = uzunluk;
    }
}

if (enIyiBaslangic == -1)
{
    Console.WriteLine("Sonuç: Sözcük yok");
}
else
{
    Console.Write("Sözcük: |");
    for (int i = enIyiBaslangic; i < enIyiBaslangic + enIyiUzunluk; i++)
    {
        Console.Write(metin[i]);
    }
    Console.WriteLine("|");
    Console.WriteLine("Başlangıç dizini: " +
        enIyiBaslangic.ToString(CultureInfo.InvariantCulture));
    Console.WriteLine("Uzunluk: " +
        enIyiUzunluk.ToString(CultureInfo.InvariantCulture));
}
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. Dizin sınırı `&&` içinde karakter erişiminden önce değerlendirilir. Son boşluklar tüketildiğinde uzunluk 0 olur; var olan aday değişmez. Çıktı döngüsü yeni sözcük aramaz, kaydedilmiş aralığı aynen yazar.

## Örnek çalıştırmalar

Bloklar `Metin: ` isteminden sonraki tam sonuçtur.

`  bir uzun  iki`:

```text
Sözcük: |uzun|
Başlangıç dizini: 6
Uzunluk: 4
```

`Ali Veli Ece`:

```text
Sözcük: |Veli|
Başlangıç dizini: 4
Uzunluk: 4
```

`ab cd`:

```text
Sözcük: |ab|
Başlangıç dizini: 0
Uzunluk: 2
```

Boş satır veya üç boşluk:

```text
Sonuç: Sözcük yok
```

| Girdi | Beklenen özellik |
| --- | --- |
| 200 adet z | Tek sözcük; başlangıç 0, uzunluk 200. |
| 199 boşluk ve A | Başlangıç 199, uzunluk 1. |
| abc123, ç, sekme, 201 karakter veya EOF | Yalnız tanımlanan hata satırı. |

## Sınır durumları

- Sözcük yokken -1 gerçek bir dizin olarak kullanılmaz.
- Son sözcükten sonra boşluk bulunması gerekmez.
- Başlangıç ve uzunluk karakter konumudur; sözcük sıra numarası değildir.
- Eşit uzunlukta güncelleme yapılmaması ilk sonucu korur.
- Yalnız boşluklar giriş hatası değildir.
- Başlangıç boşlukları özgün dizinde sayılır.

## Kazanımlar

- Ayrıştırılacak bölümün başlangıcını ve bitişini farklı anlarda belirleme.
- Sözcük üretmeden sınırları saklayarak aday seçme.
- Ön işleme adımının özgün konum bilgisini nasıl değiştirdiğini açıklama.
- Eşit aday kuralını karşılaştırmanın kesinliğiyle uygulama.
- İç döngülerin dış taramayı neden ilerlettiğini gösterme.

## Alıştırmalar

1. Eşit uzunlukta son sözcüğü seçen sürümü yazın; değişen koşulu `ab cd` ile doğrulayın.
2. En kısa sözcüğü bulun; son boşlukların 0 uzunluklu aday olarak seçilmesini önleyin.
3. Bütün en uzun sözcüklerin özgün başlangıç dizinlerini ikinci taramada yazdırın.
