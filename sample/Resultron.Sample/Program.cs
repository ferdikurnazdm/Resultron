using Resultron;
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



Console.WriteLine("\n=== Async Result Kullanımı ===");

// Async TryAsync
var tryResult = await Result.TryAsync(async () =>
{
    await Task.Delay(100);

    Console.WriteLine("[OK] Async operasyon başarıyla tamamlandı.");
});

tryResult.Match(
    onSuccess: () => Console.WriteLine("[OK] TryAsync başarılı."),
    onFailure: error => Console.WriteLine($"[HATA] {error.Code}: {error.Description}")
);


// Async TryAsync hata durumu
var failedTryResult = await Result.TryAsync(async () =>
{
    await Task.Delay(100);

    throw new InvalidOperationException("Async işlem başarısız.");
});

await failedTryResult.MatchAsync(
    onSuccess: () =>
    {
        Console.WriteLine("[OK] İşlem başarılı.");
        return Task.CompletedTask;
    },
    onFailure: error =>
    {
        Console.WriteLine($"[HATA] {error.Code}: {error.Description}");
        return Task.CompletedTask;
    });


// Async Bind zinciri
Console.WriteLine("\n=== Async Bind Zinciri ===");

var asyncCreateResult = await service.CreateAsync(
    "Async Kullanıcı",
    "async@example.com");

var asyncUpdateResult = await asyncCreateResult
    .BindAsync(user =>
    {
        Console.WriteLine($"[OK] Oluşturuldu: {user.Name}");

        return service.UpdateNameAsync(
            user.Id,
            "Async Güncellendi");
    });


await asyncUpdateResult.MatchAsync(
    onSuccess: user =>
    {
        Console.WriteLine($"[OK] Güncellendi: {user.Name}");
        return Task.CompletedTask;
    },
    onFailure: error =>
    {
        Console.WriteLine($"[HATA] {error.Code}: {error.Description}");
        return Task.CompletedTask;
    });


// Async Map
Console.WriteLine("\n=== Async Map ===");

var userNameResult = await asyncCreateResult
    .MapAsync(async user =>
    {
        await Task.Delay(50);

        return user.Name.ToUpper();
    });


await userNameResult.MatchAsync(
    name =>
    {
        Console.WriteLine($"[OK] Dönüştürülen değer: {name}");
        return Task.CompletedTask;
    },
    error =>
    {
        Console.WriteLine($"[HATA] {error.Code}: {error.Description}");
        return Task.CompletedTask;
    });


// Result<T> failure üzerinde async zincir
Console.WriteLine("\n=== Async Failure Propagation ===");

var failedUser = await service.GetByIdAsync(Guid.NewGuid());

var failedUpdate = await failedUser
    .BindAsync(user =>
    {
        return service.UpdateNameAsync(
            user.Id,
            "Yeni İsim");
    });


await failedUpdate.MatchAsync(
    _ =>
    {
        Console.WriteLine("[OK] Güncellendi.");
        return Task.CompletedTask;
    },
    error =>
    {
        Console.WriteLine($"[HATA] Zincir durdu: {error.Code} - {error.Description}");
        return Task.CompletedTask;
    });