---
id: "062"
order: 62
title: "Boşlukları normalleştirme ve sözcük sayma"
level: "İleri"
prerequisites: ["050", "061"]
concepts: ["Karakter tamponu", "Okuma ve yazma konumu", "Sözcük başlangıcı", "Ertelenen boşluk", "Geçerli uzunluk"]
---

# Boşlukları normalleştirme ve sözcük sayma

## Problem tanımı

En fazla 200 karakterlik bir satırın başındaki ve sonundaki normal boşlukları kaldırın; sözcükler arasındaki her boşluk grubunu tek boşluğa indirin. Aynı taramada sözcük adedini bulun. Sözcük, boşluk içermeyen en uzun kesintisiz harf/rakam bölümüdür. Örneğin başında iki, ortasında üç ve sonunda iki boşluk bulunan `  Ali   42  ` girdisi `Ali 42` ve 2 sözcük üretir.

Yalnız A–Z, a–z, 0–9 ve normal boşluk U+0020 kabul edilir. Boş satır ve yalnız boşluklardan oluşan satır geçerlidir; sonuç metni boş ve sözcük adedi 0 olur. Harflerin büyüklüğü ve rakamlar aynen korunur. `Trim`, `Split`, `Replace`, düzenli ifade veya hazır sözcük sayma kullanmayın. Giriş uzunluğunda bir `char` tamponu ve ayrı yazma konumu kullanın.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `metin` | 0..200 karakter; yalnız belirtilen harf, rakam ve boşluklar. |
| Ara değer | `tampon`, `yazma` | Fiziksel kapasite ve üretilmiş geçerli karakter adedi. |
| Çıktı | Normal metin | `Metin:` ardından iki dikey çubuk arasında sonuç; çubuklar çıktı ayraçlarıdır. |
| Çıktı | Sözcük adedi | `Sözcük: N`. |

`Metin: ` istemini tek satırda yanıtlayın. EOF, 201 veya daha fazla karakter, noktalama, Türkçe harf, sekme veya diğer karakter için yalnız `Hata: Metin 0..200 ASCII harf, rakam veya boşluk içermelidir.` yazılır. Bütün giriş doğrulanmadan çıktı oluşturulmaz.

## Algoritma

1. Satırı ve bütün karakterlerini doğrulayın; giriş uzunluğunda tampon oluşturun.
2. Yazma konumu ile sözcük sayısını 0, sözcük içinde olma durumunu yanlış yapın.
3. Boşlukta sözcük içinde olma durumunu yanlış yapın; henüz boşluk yazmayın.
4. Boşluk olmayan karakterde yeni sözcük başlıyorsa sayacı artırın; önceki çıktı varsa bir boşluk yazın.
5. Karakteri tampona yazıp yazma konumunu artırın; sözcük içinde olma durumunu doğru yapın.
6. Bütün giriş bitince yalnız tamponun geçerli bölümünden sonuç metnini üretip sayacı yazdırın.

Başlangıç boşlukları çıktı yokken, son boşluklar yeni sözcük başlamadan atlanır. Araya boşluk ancak sonraki sözcüğün ilk karakteri geldiğinde yazılır. Yazılmış bölümde baş/son boşluk ve çift boşluk yoktur. Sonuç uzunluğu girişten büyük olamaz; kullanılmayan tampon hücreleri çıktı değildir.

Örnekte özgün satır 12 karakter, normal sonuç 6 karakter içerir. Sonuç yazılırken yalnız bu altı tampon hücresi kullanılır. Sözcük içinde olma durumu tarama geçmişini, yazma konumu ise üretilmiş karakter adedini taşır; birbirlerinin yerine kullanılamazlar.

`  Ali   42  ` için seçilmiş adımlar:

| Okunan bölüm | Geçerli tampon | Sözcük adedi |
| --- | --- | --- |
| İlk iki boşluk | Boş | 0 |
| Ali ve sonraki üç boşluk | Ali | 1 |
| 4 | Ali 4 | 2 |
| 2 ve son iki boşluk | Ali 42 | 2 |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("Metin: ");
string? metin = Console.ReadLine();
if (metin is null || metin.Length > 200)
{
    Console.WriteLine("Hata: Metin 0..200 ASCII harf, rakam " +
        "veya boşluk içermelidir.");
    return;
}
for (int i = 0; i < metin.Length; i++)
{
    char c = metin[i];
    bool gecerli = (c >= 'A' && c <= 'Z') ||
        (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == ' ';
    if (!gecerli)
    {
        Console.WriteLine("Hata: Metin 0..200 ASCII harf, rakam " +
            "veya boşluk içermelidir.");
        return;
    }
}

char[] tampon = new char[metin.Length];
int yazma = 0, sozcuk = 0;
bool sozcukIcinde = false;
for (int i = 0; i < metin.Length; i++)
{
    char c = metin[i];
    if (c == ' ')
    {
        sozcukIcinde = false;
    }

    else
    {
        if (!sozcukIcinde)
        {
            sozcuk++;
            if (yazma > 0)
            {
                tampon[yazma++] = ' ';
            }
        }

        tampon[yazma++] = c;
        sozcukIcinde = true;
    }
}

string sonuc = new string(tampon, 0, yazma);
Console.WriteLine("Metin: |" + sonuc + "|");
Console.WriteLine("Sözcük: " + sozcuk.ToString(CultureInfo.InvariantCulture));
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. `new string(tampon, 0, yazma)` hazır normalleştirme yapmaz; algoritmanın ürettiği karakterleri metne paketler. Tamponun başlangıçtaki `\0` hücreleri yazma sınırının dışında kalır. `yazma++` atamada mevcut konumu kullanır, sonra ilerler.

## Örnek çalıştırmalar

Bloklar giriş isteminden sonraki bütün sonuç satırlarıdır; çıktıdaki çubukların arasındaki metin normalleştirilmiştir.

`  Ali   42  `:

```text
Metin: |Ali 42|
Sözcük: 2
```

`A B C`:

```text
Metin: |A B C|
Sözcük: 3
```

Boş satır veya üç boşluk:

```text
Metin: ||
Sözcük: 0
```

`x7`:

```text
Metin: |x7|
Sözcük: 1
```

| Girdi | Beklenen özellik |
| --- | --- |
| 200 adet boşluk | Geçerli uzunluk 0; sözcük adedi 0. |
| 200 adet a | Sonuç 200 adet a; tek sözcük. |
| a ve boşluk dönüşümlü 200 karakter | Son boşluk atlanır; sonuç uzunluğu 199, sözcük adedi 100. |
| Ali!, ç, sekme, 201 karakter veya EOF | Yalnız tanımlanan hata satırı. |

## Sınır durumları

- Boş girişte sıfır uzunluklu tampon ve boş `string` geçerlidir.
- Birden çok boşluk tek bir sözcük geçişi oluşturur.
- Sona boşluk yazılmasını önlemek için yazma bir sonraki sözcüğe ertelenir.
- Rakam içeren kesintisiz bölüm tek sözcüktür; `a1b2` parçalanmaz.
- Normalleştirilmiş metni tekrar işlemek aynı metni ve aynı sözcük adedini üretir.
- Geçerli veri sayısı tampon kapasitesinden küçük olabilir.

## Kazanımlar

- Metinde sözcük başlangıcını önceki tarama durumuyla tanıma.
- Okuma ve yazma konumlarını ayrı tutarak veriyi sıkıştırma.
- Sonda istenmeyen ayraç üretmemek için yazma zamanını seçme.
- Karakter tamponunun yalnız geçerli bölümünü metne dönüştürme.
- Aynı girdinin ikinci işlenişinde değişmemesini kontrol etme.

## Alıştırmalar

1. Her atlanan boşluğu sayın; giriş ve sonuç uzunluğu farkıyla karşılaştırın.
2. Boşluk yerine sözcükler arasına tek `-` yazın; boş ve tek sözcüklü girdileri koruyun.
3. Giriş tamponunu ayrı sonuç tamponu kullanmadan normalleştiren sürüm tasarlayın; okuma konumunun henüz okunmamış veriyi neden koruduğunu açıklayın.
