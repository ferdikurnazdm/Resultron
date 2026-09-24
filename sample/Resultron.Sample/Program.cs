using Resultron;
using Resultron.Sample.Repositories;
using Resultron.Sample.Services;

var repository = new InMemoryUserRepository();
var service = new UserService(repository);

Console.WriteLine("=== Kullanıcı Oluşturma ===");

var createResult = service.Create(
    "Arda Terekeci",
    "arda@example.com");

createResult.Match(
    onSuccess: user =>
    {
        Console.WriteLine(
            $"[OK] Kullanıcı oluşturuldu: {user.Id} - {user.Name}");

        return Unit.Value;
    },
    onFailure: error =>
    {
        Console.WriteLine(
            $"[HATA] {error.Code}: {error.Description}");

        return Unit.Value;
    });

var duplicateResult = service.Create(
    "Başka Biri",
    "arda@example.com");

duplicateResult.Match(
    onSuccess: user =>
    {
        Console.WriteLine($"[OK] {user.Name}");
        return Unit.Value;
    },
    onFailure: error =>
    {
        Console.WriteLine(
            $"[HATA] {error.Code}: {error.Description}");

        return Unit.Value;
    });

var invalidResult = service.Create(
    "A",
    "a@example.com");

invalidResult.Match(
    onSuccess: user =>
    {
        Console.WriteLine($"[OK] {user.Name}");
        return Unit.Value;
    },
    onFailure: error =>
    {
        Console.WriteLine(
            $"[HATA] {error.Code}: {error.Description}");

        return Unit.Value;
    });


Console.WriteLine("\n=== Kullanıcı Getirme ===");

var userId = createResult.Value.Id;

var getResult = service.GetById(userId);

getResult.Match(
    onSuccess: user =>
    {
        Console.WriteLine(
            $"[OK] Bulundu: {user.Name} - {user.Email}");

        return Unit.Value;
    },
    onFailure: error =>
    {
        Console.WriteLine(
            $"[HATA] {error.Code}: {error.Description}");

        return Unit.Value;
    });


var notFoundResult = service.GetById(
    Guid.NewGuid());

notFoundResult.Match(
    onSuccess: user =>
    {
        Console.WriteLine($"[OK] {user.Name}");
        return Unit.Value;
    },
    onFailure: error =>
    {
        Console.WriteLine(
            $"[HATA] {error.Code}: {error.Description}");

        return Unit.Value;
    });


Console.WriteLine(
    "\n=== İsim Güncelleme (Map + Bind zinciri) ===");

var updateResult = service.UpdateName(
    userId,
    "Arda T.");

updateResult.Match(
    onSuccess: user =>
    {
        Console.WriteLine(
            $"[OK] Güncellendi: {user.Name}");

        return Unit.Value;
    },
    onFailure: error =>
    {
        Console.WriteLine(
            $"[HATA] {error.Code}: {error.Description}");

        return Unit.Value;
    });


var invalidUpdateResult = service.UpdateName(
    userId,
    "X");

invalidUpdateResult.Match(
    onSuccess: user =>
    {
        Console.WriteLine($"[OK] {user.Name}");
        return Unit.Value;
    },
    onFailure: error =>
    {
        Console.WriteLine(
            $"[HATA] {error.Code}: {error.Description}");

        return Unit.Value;
    });


Console.WriteLine("\n=== Kullanıcı Silme ===");

var deleteResult = service.Delete(userId);

deleteResult.Match(
    onSuccess: () =>
    {
        Console.WriteLine(
            "[OK] Kullanıcı silindi.");

        return Unit.Value;
    },
    onFailure: error =>
    {
        Console.WriteLine(
            $"[HATA] {error.Code}: {error.Description}");

        return Unit.Value;
    });


var deleteAgainResult = service.Delete(userId);

deleteAgainResult.Match(
    onSuccess: () =>
    {
        Console.WriteLine("[OK] Silindi.");
        return Unit.Value;
    },
    onFailure: error =>
    {
        Console.WriteLine(
            $"[HATA] {error.Code}: {error.Description}");

        return Unit.Value;
    });


Console.WriteLine("\n=== Async Result Kullanımı ===");

var tryResult = await Result.TryAsync(
    async () =>
    {
        await Task.Delay(100);

        Console.WriteLine(
            "[OK] Operasyon başarıyla tamamlandı.");
    });

tryResult.Match(
    onSuccess: () =>
    {
        Console.WriteLine(
            "[OK] TryAsync başarılı.");

        return Unit.Value;
    },
    onFailure: error =>
    {
        Console.WriteLine(
            $"[HATA] {error.Code}: {error.Description}");

        return Unit.Value;
    });


var failedTryResult = await Result.TryAsync(
    async () =>
    {
        await Task.Delay(100);

        throw new InvalidOperationException(
            "Async işlem başarısız.");
    });

failedTryResult.Match(
    onSuccess: () =>
    {
        Console.WriteLine(
            "[OK] İşlem başarılı.");

        return Unit.Value;
    },
    onFailure: error =>
    {
        Console.WriteLine(
            $"[HATA] {error.Code}: {error.Description}");

        return Unit.Value;
    });


Console.WriteLine("\n=== Async Bind Zinciri ===");

var asyncCreateResult = await service.CreateAsync(
    "Async Kullanıcı",
    "async@example.com");

var asyncUpdateResult = await asyncCreateResult
    .BindAsync(user =>
    {
        Console.WriteLine(
            $"[OK] Oluşturuldu: {user.Name}");

        return service.UpdateNameAsync(
            user.Id,
            "Async Güncellendi");
    });


// MatchAsync artık Task<Result<T>> extension'ı.
// Elimizde Result<T> olduğu için Task.FromResult ile Task'e çeviriyoruz.

_ = await Task.FromResult(asyncUpdateResult)
    .MatchAsync(
        onSuccess: user =>
        {
            Console.WriteLine(
                $"[OK] Güncellendi: {user.Name}");

            return Task.FromResult(Unit.Value);
        },
        onFailure: error =>
        {
            Console.WriteLine(
                $"[HATA] {error.Code}: {error.Description}");

            return Task.FromResult(Unit.Value);
        });


Console.WriteLine("\n=== Async Map ===");

// Map sonucu string olduğu için Result<string>.

var userNameResult =
    await asyncCreateResult.MapAsync(
        async user =>
        {
            await Task.Delay(50);

            return user.Name.ToUpper();
        });


_ = await Task.FromResult(userNameResult)
    .MatchAsync(
        onSuccess: name =>
        {
            Console.WriteLine(
                $"[OK] Dönüştürülen değer: {name}");

            return Task.FromResult(Unit.Value);
        },
        onFailure: error =>
        {
            Console.WriteLine(
                $"[HATA] {error.Code}: {error.Description}");

            return Task.FromResult(Unit.Value);
        });


Console.WriteLine(
    "\n=== Async Failure Propagation ===");

var failedUser = await service.GetByIdAsync(
    Guid.NewGuid());

var failedUpdate = await failedUser
    .BindAsync(user =>
        service.UpdateNameAsync(
            user.Id,
            "Yeni İsim"));


_ = await Task.FromResult(failedUpdate)
    .MatchAsync(
        onSuccess: _ =>
        {
            Console.WriteLine(
                "[OK] Güncellendi.");

            return Task.FromResult(Unit.Value);
        },
        onFailure: error =>
        {
            Console.WriteLine(
                $"[HATA] Zincir durdu: " +
                $"{error.Code} - {error.Description}");

            return Task.FromResult(Unit.Value);
        });