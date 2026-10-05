---
id: "070"
order: 70
title: "Yığınla farklı parantez türlerini eşleştirme"
level: "İleri"
prerequisites: ["050", "064", "069"]
concepts: ["Diziyle yığın", "Son giren ilk çıkar", "İç içe eşleşme", "İlk hata konumu", "En yüksek derinlik"]
---

# Yığınla farklı parantez türlerini eşleştirme

## Problem tanımı

En fazla 200 yazdırılabilir ASCII karakteri içeren satırda `()`, `[]` ve `{}` parantezlerinin doğru tür ve sırayla eşleşmesini denetleyin. Açılan son parantez önce kapanmalıdır. Dengeli girdide en yüksek iç içe parantez derinliğini yazdırın. Dengesiz girdide ilk sorunlu dizini ve nedenini bildirin. Örneğin `([)]` içinde adetler eşit olsa da 2. dizindeki `)` en son açılmış `[` ile uyuşmaz.

Bu problem yalnız parantez eşleştirmesidir: parantez olmayan bütün kabul edilmiş karakterler yok sayılır. Tırnak, yorum veya kaçış dizileri için ayrı dil kuralı yoktur; tırnak içindeki parantezler de denetlenir. Boş satır ve hiç parantez içermeyen satır dengeli, derinliği 0 kabul edilir. Hazır `Stack`, düzenli ifade veya ifade ayrıştırıcı kullanmayın; `char` dizisi ve geçerli eleman adediyle yığın kurun.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `metin` | 0..200 karakter; ASCII 32..126. |
| Ara değer | `yigin`, `tepe` | Henüz kapanmamış açılışlar ve geçerli eleman adedi. |
| Çıktı | Dengeli | `Dengeli: Evet`, ardından `En yüksek derinlik: D`. |
| Çıktı | Dengesiz | `Dengeli: Hayır`, `Hata dizini: I`, `Neden: açıklama`. |

`İfade: ` istemini tek satırla yanıtlayın. Bir kapanış için yığın boşsa neden `Açılış yok`, tepesi başka türdeyse `Tür uyuşmazlığı` olur; dizin o kapanışın özgün sıfır tabanlı konumudur. Giriş bittiğinde açık parantez varsa neden `Kapanış eksik`, hata dizini metin uzunluğudur; bu değer giriş sonu konumunu belirtir ve karakter erişiminde kullanılmaz.

EOF, uzunluk ihlali, sekme, Türkçe harf veya diğer kabul edilmeyen karakter için yalnız `Hata: İfade 0..200 yazdırılabilir ASCII karakteri içermelidir.` yazılır. Bu giriş hatası, geçerli karakterlerden oluşan dengesiz ifadenin normal sonucundan ayrıdır. Girdinin tamamı önce doğrulanır.

## Algoritma

1. Satırı, uzunluğunu ve bütün karakterlerini doğrulayın.
2. Giriş uzunluğunda yığın dizisi oluşturun; geçerli adet ve en yüksek derinliği 0 yapın.
3. Açılış karakterini yığının sonraki boş hücresine yazıp adedi artırın; en yüksek adedi güncelleyin.
4. Kapanış karakterinde yığın boşsa açılış yok sonucuyla taramayı durdurun.
5. Beklenen açılış türünü hesaplayın; tepedeki tür farklıysa uyuşmazlık sonucuyla durdurun.
6. Tür doğruysa geçerli adedi 1 azaltıp son açılışı çıkarın; diğer karakterleri atlayın.
7. Tarama hatasız bittiğinde yığında eleman kaldıysa giriş sonu için kapanış eksik sonucunu üretin.
8. Hata durumunu veya dengeli kararını ve en yüksek derinliği yazdırın.

Her adım başında yığında, işlenmiş bölümün henüz kapanmamış açılışları giriş sırasıyla bulunur. Doğru kapanış yalnız en son açılışı tüketir; farklı bir alt açılışı seçmek iç içe düzeni bozar. `tepe` kapasite değil geçerli adettir; boş değilken üst karakter `yigin[tepe - 1]` olur. Bir karakter en fazla bir kez yığına girer; giriş uzunluğunda kapasite yeterlidir.

`([)]` için:

| Dizin | Karakter | Yığın; soldan sağa alt-üst | İşlem |
| --- | --- | --- | --- |
| 0 | `(` | `(` | Ekle; derinlik 1 |
| 1 | `[` | `([` | Ekle; derinlik 2 |
| 2 | `)` | `([` | Tepedeki `[` uymaz; dur |

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("İfade: ");
string? metin = Console.ReadLine();
if (metin is null || metin.Length > 200)
{
    Console.WriteLine("Hata: İfade 0..200 yazdırılabilir ASCII " +
        "karakteri içermelidir.");
    return;
}
for (int i = 0; i < metin.Length; i++)
{
    if (metin[i] < ' ' || metin[i] > '~')
    {
        Console.WriteLine("Hata: İfade 0..200 yazdırılabilir ASCII " +
            "karakteri içermelidir.");
        return;
    }
}

char[] yigin = new char[metin.Length];
int tepe = 0, enYuksek = 0, hataDizini = -1;
string neden = "";
for (int i = 0; i < metin.Length; i++)
{
    char c = metin[i];
    if (c == '(' || c == '[' || c == '{')
    {
        yigin[tepe++] = c;
        if (tepe > enYuksek)
        {
            enYuksek = tepe;
        }
    }

    else if (c == ')' || c == ']' || c == '}')
    {
        if (tepe == 0)
        {
            hataDizini = i;
            neden = "Açılış yok";
            break;
        }

        char beklenen = c == ')' ? '(' : c == ']' ? '[' : '{';
        if (yigin[tepe - 1] != beklenen)
        {
            hataDizini = i;
            neden = "Tür uyuşmazlığı";
            break;
        }
        tepe--;
    }
}

if (hataDizini == -1 && tepe > 0)
{
    hataDizini = metin.Length;
    neden = "Kapanış eksik";
}
if (hataDizini == -1)
{
    Console.WriteLine("Dengeli: Evet");
    Console.WriteLine("En yüksek derinlik: " +
        enYuksek.ToString(CultureInfo.InvariantCulture));
}
else
{
    Console.WriteLine("Dengeli: Hayır");
    Console.WriteLine("Hata dizini: " +
        hataDizini.ToString(CultureInfo.InvariantCulture));
    Console.WriteLine("Neden: " + neden);
}
```

Kod bağımsız konsol projesinin `Program.cs` dosyasında çalışır. Çıkarma eski hücreyi silmez; `tepe` azaldığı için o hücre geçerli yığının dışına çıkar. Sonraki ekleme aynı hücreyi yeniden kullanabilir. `hataDizini == -1` kontrolü, ilk kapanış hatasının giriş sonu hatasıyla değiştirilmesini önler. Dengesiz sonuçta derinlik satırı yazılmaz.

## Örnek çalıştırmalar

Bloklar `İfade: ` isteminden sonraki tam sonuçtur.

`(a+[b*c])`:

```text
Dengeli: Evet
En yüksek derinlik: 2
```

`([)]`:

```text
Dengeli: Hayır
Hata dizini: 2
Neden: Tür uyuşmazlığı
```

`a)`:

```text
Dengeli: Hayır
Hata dizini: 1
Neden: Açılış yok
```

`(a`:

```text
Dengeli: Hayır
Hata dizini: 2
Neden: Kapanış eksik
```

Boş satır veya `abc 123!`:

```text
Dengeli: Evet
En yüksek derinlik: 0
```

| Girdi | Beklenen özellik |
| --- | --- |
| `()[]{}` | Dengeli; en yüksek derinlik 1. |
| 100 adet ( ardından 100 adet ) | Dengeli; en yüksek derinlik 100. |
| 200 adet ( | Kapanış eksik; hata dizini 200, kapasite aşılmaz. |
| `([)` ve ardından sekme | Karakter doğrulama hatası; önceki uyuşmazlık sonucu yazılmaz. |
| 201 karakter, Türkçe harf veya EOF | Yalnız tanımlanan giriş hata satırı. |

## Sınır durumları

- Eşit açılış/kapanış adetleri doğru tür ve sıra için yeterli değildir.
- Boş yığında `tepe - 1` erişimi yapılmaz.
- Sonradan gelen başka hatalar ilk kapanış hatasını değiştirmez.
- Giriş sonu dizini bir karakterin dizini değildir.
- Boş veya parantezsiz geçerli satır dengelidir.
- Yığında kalan eski hücreler geçerli eleman adedine dahil edilmez.
- Tırnak ve yorum ayrıştırılması bu problemin kuralı değildir; tüm parantezler aynı değerlendirilir.
- En yüksek derinlik anlık açık adetlerinin en büyüğüdür; toplam çift sayısı değildir.

## Kazanımlar

- Son giren ilk çıkar davranışını hazır koleksiyon olmadan diziyle gerçekleştirme.
- Fiziksel kapasite, geçerli adet ve tepe dizinini ayırt etme.
- Birden fazla parantez türünün iç içe düzenini koruma.
- Geçersiz giriş ile geçerli fakat dengesiz ifade sonucunu ayırma.
- İlk hata konumunu ve giriş sonu hatasını farklı anlamlarla raporlama.

## Alıştırmalar

1. Yığına açılışların özgün dizinlerini de koyun; kapanış eksikse en son açık parantezin dizinini ayrıca yazdırın.
2. Dengeli ifadede toplam eşleşmiş çift sayısını sayın; derinlikle aynı olmadığını ()() ve (()) ile gösterin.
3. Yalnız tek tür parantez için bir sayaçla çalışan sürüm yazın; bunun ([)] durumunu neden çözemediğini açıklayın.
