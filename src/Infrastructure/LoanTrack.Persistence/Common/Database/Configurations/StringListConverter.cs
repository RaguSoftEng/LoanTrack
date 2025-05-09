using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LoanTrack.Persistence.Common.Database.Configurations;

public class StringListConverter() : ValueConverter<List<string>, string>(
    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null));
