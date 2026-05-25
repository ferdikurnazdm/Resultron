using Resultron.Sample.Repositories;
using Resultron.Sample.Services;

var repository = new InMemoryUserRepository();
var service = new UserService(repository);

Console.WriteLine("=== Kullanıcı Oluşturma ===");

// Başarılı oluşturma
var createResult = service.Create("Arda Terekeci", "arda@example.com");
createResult.Match(
    onSuccess: user => Console.WriteLine($"[OK] Kullanıcı oluşturuldu: {user.Id} - {user.Name}"),
    onFailure: error => Console.WriteLine($"[HATA] {error.Code}: {error.Description}")
);

// Aynı email ile tekrar oluşturma
var duplicateResult = service.Create("Başka Biri", "arda@example.com");
duplicateResult.Match(
    onSuccess: user => Console.WriteLine($"[OK] {user.Name}"),
    onFailure: error => Console.WriteLine($"[HATA] {error.Code}: {error.Description}")
);

// Geçersiz isim
var invalidResult = service.Create("A", "a@example.com");
invalidResult.Match(
    onSuccess: user => Console.WriteLine($"[OK] {user.Name}"),
    onFailure: error => Console.WriteLine($"[HATA] {error.Code}: {error.Description}")
);

Console.WriteLine("\n=== Kullanıcı Getirme ===");

var userId = createResult.Value.Id;
var getResult = service.GetById(userId);
getResult.Match(
    onSuccess: user => Console.WriteLine($"[OK] Bulundu: {user.Name} - {user.Email}"),
    onFailure: error => Console.WriteLine($"[HATA] {error.Code}: {error.Description}")
);

// Olmayan kullanıcı
var notFoundResult = service.GetById(Guid.NewGuid());
notFoundResult.Match(
    onSuccess: user => Console.WriteLine($"[OK] {user.Name}"),
    onFailure: error => Console.WriteLine($"[HATA] {error.Code}: {error.Description}")
);

Console.WriteLine("\n=== İsim Güncelleme (Map + Bind zinciri) ===");

var updateResult = service.UpdateName(userId, "Arda T.");
updateResult.Match(
    onSuccess: user => Console.WriteLine($"[OK] Güncellendi: {user.Name}"),
    onFailure: error => Console.WriteLine($"[HATA] {error.Code}: {error.Description}")
);

// Geçersiz isim ile güncelleme
var invalidUpdateResult = service.UpdateName(userId, "X");
invalidUpdateResult.Match(
    onSuccess: user => Console.WriteLine($"[OK] {user.Name}"),
    onFailure: error => Console.WriteLine($"[HATA] {error.Code}: {error.Description}")
);

Console.WriteLine("\n=== Kullanıcı Silme ===");

var deleteResult = service.Delete(userId);
deleteResult.Match(
    onSuccess: () => Console.WriteLine("[OK] Kullanıcı silindi."),
    onFailure: error => Console.WriteLine($"[HATA] {error.Code}: {error.Description}")
);

// Tekrar silmeye çalışma
var deleteAgainResult = service.Delete(userId);
deleteAgainResult.Match(
    onSuccess: () => Console.WriteLine("[OK] Silindi."),
    onFailure: error => Console.WriteLine($"[HATA] {error.Code}: {error.Description}")
);
