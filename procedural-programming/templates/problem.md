---
id: "100"
order: 100
title: "Problem başlığı"
level: "İleri"
prerequisites: ["089", "090"]
concepts: ["Yeni ana kavram"]
---

# Problem başlığı

## Problem tanımı

Çözülecek durumu, amacı ve varsayımları açıklayın. Öğrenci bu dosyayı tek başına okuyabilmelidir.

001–100 kaynakları hazırlanmıştır ve kitap toplam 100 soruyla tamamlanmıştır. Bu şablon ileride ayrı bir çalışma için kullanılabilir; üst bilgideki örnek kimlik mevcut bir soruyla çakışır. Önkoşulları bu örneği aynen kopyalayarak değil, problemin gerçekten kullandığı önceki kazanımlara göre seçin. Veri ve davranış tekrarlarını nesneye yönelik programlamaya geçiş gerekçesiyle ilişkilendirin; çözüm static yerel fonksiyonlarla bağımsız prosedürel bir konsol programı olarak kalsın.

## Girdi ve çıktı

| Tür | Değer | Açıklama |
| --- | --- | --- |
| Girdi | Değişken adı | Türü, birimi ve kabul edilen aralığı |
| Çıktı | Sonuç adı | Beklenen biçim ve anlamı |

Dosya kullanan örnekte tüm yolları ve komut satırı argümanlarını, başlık/ayırıcı/tırnak kurallarını, UTF-8 kodlamasını, sayı ve tarih biçimini, fiziksel satır ve alan konumlarını belirtin. EOF, boş satır, boş/eksik dosya, geçersiz kayıt, yinelenen veya eksik ilişkili anahtar, kapasite ve taşma davranışlarını tanımlayın. Bir örnek çalıştırma için gereken her dosyanın tam içeriğini verin. Çıktı yazılacaksa mevcut kullanıcı dosyasını ezmeyen bir sözleşme ve I/O hatasında kısmi yeni dosyanın durumunu açıklayın.

## Algoritma

1. Girdiyi okuyun.
2. Girdiyi doğrulayın.
3. Problemi çözen adımları sırayla uygulayın.
4. Sonucu yazdırın.

Gerekiyorsa bu bölümde `diagram` bloğu veya yerel resim bağlantısı kullanın. Örnekler için 004, 005 ve 008 dosyalarına bakın.

## C# çözümü

Aşağıdaki yer tutucuyu tam ve bağımsız bir konsol çözümüyle değiştirin.

```csharp
using System;

Console.WriteLine("Şablonu tamamlayın.");
```

## Örnek çalıştırmalar

| Girdi | Sonuç | Açıklama |
| --- | --- | --- |
| Normal örnek | Kodun ürettiği tam sonuç | Neden bu sonuç çıkar |
| Sınır örneği | Kodun ürettiği tam sonuç | Hangi sınır denenir |

## Sınır durumları

- Alt ve üst girdi sınırlarını açıklayın.
- Geçersiz girdide programın davranışını belirtin.
- Gerekli ise taşma, sıfıra bölme veya boş veri durumunu ele alın.
- Dosya kaynaklarını using ile kapatın; testleri izole geçici verilerde yürütün. Gerçek Markdown C# bloğunu derleyin; normal, sınır ve geçersiz örnekleri bağımsız beklenen sonuçlarla karşılaştırın. Hazır LINQ/gruplama/sıralama araçlarıyla öğretilen algoritmayı atlamayın.

## Kazanımlar

- Öğrenci tarafından gösterilebilir, somut kazanımları yazın.
- Ana kavramın bu problemde nasıl kullanıldığını açıklayın.

## Alıştırmalar

1. Aynı kazanımı pekiştiren küçük bir değişiklik isteyin.
2. Bir sınır durumunun neden özel olduğunu açıklamasını isteyin.
