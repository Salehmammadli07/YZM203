# 100 soruluk kitabı tamamlama görevleri

Bu plan, 5 Ekim 2026 tarihinde kullanıcı isteğiyle oluşturulan dört yerel Codex otomasyonunu tanımlar. Görevler YZM205 projesinin mevcut çalışma klasöründe çalışır. Markdown kaynakları korunur; soru çözümleri ve belge derleyicisi C# kullanır.

## Çalışma sırası

| Aşama | Yerel görev | Otomasyon kimliği | Başlama koşulu |
| --- | --- | --- | --- |
| 1 | 081–090: dosyalar ve birden çok veri kümesi | `prosed-rel-kitap-081-090-sorular` | 5 Ekim 2026, 00.30; Europe/Istanbul |
| 2 | 091–100: birleşik prosedürel uygulamalar | `prosed-rel-kitap-091-100-sorular` | 1. aşama başarıyla tamamlandıktan en az 60 dakika sonra |
| 3 | Prof. Dr. Zafer CÖMERT'in Önsözü ve Giriş | `prosed-rel-kitap-ns-z-ve-giri` | 100 soru doğrulanıp 2. aşama tamamlandıktan en az 60 dakika sonra |
| 4 | Kitabın son teknik ve editoryal kontrolleri | `prosed-rel-kitap-son-kontroller` | 3. aşama başarıyla tamamlandıktan en az 60 dakika sonra |

İlk otomasyon etkinleştirilir. Sonraki üç otomasyon, önceki aşamanın devreye almasını bekler. Bir aşama başarıyla tamamlandığında sonraki otomasyonu kendisi etkinleştirir ve onun ilk çalışmasını kendi tamamlanma zamanından en az bir saat sonraya ayarlar. Böylece görevler çakışmaz. Saatler ilk uygun tam dakikaya yuvarlanır; tarih geçişleri Europe/Istanbul saat diliminde hesaplanır.

Görevler sonludur. Tamamlanan otomasyon duraklatılır. Eksik gereksinim veya başarısız doğrulamada aşama tamamlandı sayılmaz; ilgili otomasyon bir saat sonraya yeniden zamanlanır ve mevcut çalışmayı sürdürür. Devir başarısızsa yalnız devir işlemi yeniden denenir. Sonraki aşama, önceki aşamanın sonucunu ve bir saatlik boşluğu ayrıca doğrular.

## İçerik ve teslim ölçütleri

1. **081–090:** Önce on özgün problem seçilir ve kademeli sıralanır. Dosya biçimi, satır/alan doğrulama, hata konumu, kaynak kapatma ve birden çok veri kümesi işleme öğretilir. Bağımsız Markdown dosyaları, C# çözümleri, örnek dosyalar, tam çıktılar, izleme tabloları, sınır durumları, kazanımlar ve alıştırmalar hazırlanır. README, ROADMAP ve şablon güncellenir; 90 soruluk Word kitabı doğrulanır.
2. **091–100:** Daha kapsamlı prosedürel uygulamalar hazırlanır. İlişkili veriler, dosya işlemleri, metotlar ve raporlama birleştirilir. Nesneye yönelik programlamaya geçişin gerekçeleri kazanımlarda açıklanır. Aynı içerik standardı korunur; toplam tam 100 soru doğrulanır ve Word kitabı yeniden üretilir.
3. **Önsöz/Giriş:** `front-matter/01-onsoz.md` ve `front-matter/02-giris.md` kaynakları yazılır. Önsöz, kitabın amacını ve çalışma biçimini anlatır; Prof. Dr. Zafer CÖMERT imzasını taşır. Giriş; prosedürel programlamanın tanımını, neden öğrenilmesi gerektiğini, farklı paradigmaları, paradigmalara hakim olmanın önemini ve kitabın kullanımını açıklar. C# derleyicisine bu kaynakları yeniden derlenebilir ön bölümler olarak dahil etme desteği eklenir. Ön bölümler 100 sorunun dışında tutulur.
4. **Son kontrol:** Kimlikler, sıralama, önkoşullar, bütün kaynak/görsel bağlantıları, örnekler, C# çözümleri, sınır ve hata durumları denetlenir. İçindekiler, şekil dizini ve sayfa numaraları güncellenir. Son Word kitabının sayfa yerleşimleri doğrulanır; bulgular `FINAL_REVIEW.md` dosyasına kaydedilir.

Kitap sırası: kapak, Önsöz, İçindekiler, Şekil dizini, Giriş, 001–100 problemleri. Önsöz ve Giriş, problem numaralandırmasını değiştirmez.

Kullanıcının güncel bölüm düzeni: 100 problem, [chapters.md](chapters.md) başlıklarıyla onar soruluk 10 bölüm olarak derlenir. Bölümler içindekilerde birinci, problemler ikinci düzeydedir. Her bölüm yeni çift numaralı sayfadan başlamalı; sayfa numaraları kesintisiz olmalıdır. Giriş eklendikten sonra da Word'ün `evenPage` bölüm sonlarını ve bu başlık hiyerarşisini koruyun. Ön bölüm ekleme ve son kontrolde 10 bölümün başlangıç sayfalarını gerçek Word yerleşiminden okuyarak çift olduklarını doğrulayın.

## Durum ve otomasyon ayarları

Çalışma kaydı `procedural-programming/build/automation-pipeline-state.json` dosyasındadır. Bu dosya üretilen çıktılarla birlikte Git dışında kalır. `stages` nesnesinin `1`, `2`, `3`, `4` anahtarları ilgili aşamaya aittir. Her aşama kendi kaydını günceller; önceki kayıtları korur. `status`, `completedAtUtc`, `scheduledAtUtc`, `handoffStatus` ve `checks` alanları somut sonuçlarla doldurulur. Bir başarı kaydı, kaynakları ve gerçek kontrol sonuçlarını incelemenin yerine geçmez.

Otomasyonlar `kind: cron`, `destination: local`, `executionEnvironment: local`, `projectId: 8c00c46b-cda0-46f9-a7c6-2d2e84c65849`, `model: gpt-6.1-sol`, `reasoningEffort: high` ayarlarıyla oluşturulmuştur. Her birinin tam istemi uygulamanın kendi otomasyon kaydında saklanır.

Ayarlar salt okunur olarak `C:/Users/zafer/.codex/automations/<kimlik>/automation.toml` dosyasından alınabilir. Etkinleştirme, yeniden zamanlama ve duraklatma yalnız uygulamanın `automation_update` aracıyla yapılır; otomasyon kayıtları elle değiştirilmez. Güncellemede mevcut isim, istem, proje, model ve düşünme seviyesi korunur. İlk uygun zaman geçmişse gelecekteki ilk uygun tam dakika seçilir.

Yerel görevlerin çalışması için bilgisayarın ve Codex uygulamasının açık, proje klasörünün erişilebilir olması gerekir. Word alanlarının güncellenmesi için mevcut masaüstü Microsoft Word kullanılır.
