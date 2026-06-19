# 🛡️ GoodbyeDPI Manager (Türkiye Edition)

**GoodbyeDPI Manager**, Siyah pencerelerle uğraşmadan tek tıkla kullanmanızı sağlayan modern bir Windows arayüzüdür.

## 🚀 Özellikler

* **Tek Tıkla Kurulum:** Siyah CMD ekranları yok. "Aç" butonuna basarsınız, gerekli servisler otomatik kurulur.
* **Otomatik Başlatma:** İsteğe bağlı olarak Windows açıldığında otomatik devreye girer.
* **Sessiz Mod:** Uygulamayı kapattığınızda sistem tepsisine (System Tray) küçülür ve arka planda çalışmaya devam eder.
* **Canlı Durum Takibi:** Servisin çalışıp çalışmadığını anlık renkli gösterge ile takip edebilirsiniz.
* **Tek Dosya (Portable):** Kurulum gerektirmez, indirip direkt çalıştırabilirsiniz.

---

## 📥 İndirme ve Kullanım

Kodlarla uğraşmak istemiyorsanız, hazır derlenmiş sürümü indirebilirsiniz:

1.  Bu sayfanın sağ tarafındaki **[Releases (Sürümler)](../../releases)** kısmına tıklayın.
2.  En son sürümün altındaki **ZIP** dosyasını indirin.

> **Not:** Uygulama Windows Servislerini (`sc create`) yönettiği için **Yönetici İzni** şarttır.

---

## 🏆 Teşekkürler ve Kaynaklar (Credits)

Bu proje bir "GUI Wrapper" (Arayüz Giydirme) projesidir ve devlerin omuzlarında yükselmektedir. Asıl işi yapan aşağıdaki projelere sonsuz teşekkürler:

* **[GoodbyeDPI-Turkey](https://github.com/cagritaskn/GoodbyeDPI-Turkey):** Türkiye şartlarına özel ayarları yapan ve bu projeye ilham olan fork. (**cagritaskn**'e özel teşekkürler).
* **[GoodbyeDPI](https://github.com/ValdikSS/GoodbyeDPI):** Orijinal DPI atlatma aracının yaratıcısı **ValdikSS**.
* **WinDivert:** Ağ paketlerini yakalamak ve işlemek için kullanılan temel kütüphane.

---

## ⚙️ Teknik Detaylar

Bu yazılım, *GoodbyeDPI* çekirdeğini (goodbyedpi.exe, WinDivert.dll/sys) kendi içinde barındırır (Embedded Resource). Uygulama çalıştırıldığında bu dosyaları geçici olarak `C:\ProgramData\MyGoodbyeDPI` klasörüne çıkarır ve Windows Servis Yönetimi üzerinden aşağıdaki Türkiye optimize parametrelerini uygular:

`sc create "GoodbyeDPI" binPath= "... -5 --set-ttl 5 --dns-addr 77.88.8.8 --dns-port 1253 --dnsv6-addr 2a02:6b8::feed:0ff --dnsv6-port 1253 ..."`

---

## ⚠️ Yasal Uyarı

Bu yazılım; eğitim, araştırma ve bilgiye erişim özgürlüğü amaçlarıyla, açık kaynak kodlu projeler temel alınarak geliştirilmiştir. Yazılımın kullanımından doğabilecek her türlü yasal sorumluluk son kullanıcıya aittir.

License: Apache-2.0 (GoodbyeDPI-Turkey temel alınmıştır).


https://claytstudio.com/dnschangerapp/
