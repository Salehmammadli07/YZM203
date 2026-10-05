---
id: "009"
order: 9
title: "Bir sayının faktöriyelini hesaplama"
level: "Temel"
prerequisites: ["008"]
concepts: ["For döngüsü", "Çarpım biriktirme", "Başlangıç değeri", "Sayısal sınırlar"]
---

# Bir sayının faktöriyelini hesaplama

## Problem tanımı

Sıfır veya pozitif bir tamsayının faktöriyelini hesaplayın. Pozitif `N` için `N!`, 1'den `N` değerine kadar tamsayıların çarpımıdır. Örneğin `4! = 1 × 2 × 3 × 4 = 24` olur. Sıfırın faktöriyeli tanım gereği `0! = 1` kabul edilir.

Çözümde çarpım, bir döngü ile biriktirilir. Toplama biriktiricisini sıfırla başlatırken, çarpma biriktiricisini birle başlatmak gerekir. Başlangıç değeri sıfır olsaydı sonraki bütün çarpımlar sıfır kalırdı. Bu seçim, sıfır girdisini ek bir hesaplama gerektirmeden doğru sonuçlandırır.

## Girdi ve çıktı

| Tür | Ad | Açıklama |
| --- | --- | --- |
| Girdi | `n` | 0 ile 20 arasında bir tamsayı. |
| Çıktı | `faktoriyel` | `n!` değeri; `long` türünde. |

Program `N (0..20): ` istemini gösterir. Bir tamsayı yazıp Enter tuşuna basın. Basamak ayırıcı kullanmayın. İzin verilen üst sınır, sonucu `long` türünde güvenle saklayabilmek için seçilmiştir. Sayılar kültürden bağımsız biçimde okunur ve yazdırılır.

## Algoritma

1. `N` değerini okuyun.
2. Girdinin tamsayı olduğunu ve 0 ile 20 aralığında bulunduğunu doğrulayın.
3. Geçersiz girişte hata mesajı gösterip programı bitirin.
4. `faktoriyel` değişkenini 1 ile başlatın.
5. Sayacı 1'den `N` değerine kadar artırın.
6. Her adımda mevcut faktöriyel değerini sayaçla çarpın.
7. Sonucu yazdırın.

Döngüde sayaç `k` işlendiğinde biriktiricinin değeri `k!` olur. Böylece her adımın sonucunu önceki adım üzerinden açıklamak mümkündür. `N = 0` olduğunda ilk döngü koşulu yanlış olur ve başlangıç değeri 1 korunur.

## C# çözümü

```csharp
using System;
using System.Globalization;

Console.Write("N (0..20): ");
if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer,
    CultureInfo.InvariantCulture, out int n) || n < 0 || n > 20)
{
    Console.WriteLine("Hata: 0 ile 20 arasında bir tamsayı girin.");
    return;
}

long faktoriyel = 1;
for (int sayac = 1; sayac <= n; sayac++)
{
    faktoriyel *= sayac;
}

Console.WriteLine("Faktöriyel: " +
    faktoriyel.ToString(CultureInfo.InvariantCulture));
```

`faktoriyel *= sayac`, `faktoriyel = faktoriyel * sayac` ifadesine karşılık gelir. Döngü koşulunda `<=` kullanmak son çarpanı da dahil eder. Girdi doğrulaması çarpma başlamadan yapılır; bu nedenle izin verilmeyen bir sayı kısmi sonuç üretmez.

## Örnek çalıştırmalar

Tabloda istem metni yerine sonuç satırı gösterilir.

| Girdi | Sonuç |
| --- | --- |
| `0` | `Faktöriyel: 1` |
| `1` | `Faktöriyel: 1` |
| `4` | `Faktöriyel: 24` |
| `10` | `Faktöriyel: 3628800` |
| `20` | `Faktöriyel: 2432902008176640000` |

## Sınır durumları

- `0` ve `1` farklı sayıda döngü adımıyla aynı sonucu üretir.
- Negatif sayıların faktöriyeli bu problem kapsamında hesaplanmaz.
- `20!`, `long` aralığına sığar; `21!` sığmadığı için `21` reddedilir.
- `12!` değeri `int` aralığına sığsa da `13!` sığmaz; biriktirici bu yüzden `long` seçilmiştir.
- Boş veya sayısal olmayan girişte hata mesajı yazdırılır.

## Kazanımlar

- Çarpım biriktiricisinin başlangıç değerini gerekçelendirme.
- Matematiksel bir işlemi sayaçlı döngüye dönüştürme.
- Veri türünün sonuç büyüklüğünü sınırladığını fark etme.
- Döngünün hiç çalışmaması durumunu açıklama.

## Alıştırmalar

1. `N = 5` için her adımın sayaç ve çarpım değerlerini yazın.
2. Döngüyü `while` ile yeniden kurun.
3. Başlangıç değerini sıfır yapmanın sonuçlara etkisini açıklayın.
