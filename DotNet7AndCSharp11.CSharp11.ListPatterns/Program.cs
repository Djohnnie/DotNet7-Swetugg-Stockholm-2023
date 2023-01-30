using static System.Console;
using static System.String;


WriteLine("{ 1, 2, 10 }");
WriteLine("{ 1, 2, 7, 3, 3, 10 }");
WriteLine("{ 1, 2 }");
WriteLine("{ 1, 3 }");
WriteLine("{ 1, 3, 5 }");
WriteLine("{ 2, 5, 6, 7 }");

WriteLine();

WriteLine(CheckSwitch(new[] { 1, 2, 10 }));
WriteLine(CheckSwitch(new[] { 1, 2, 7, 3, 3, 10 }));
WriteLine(CheckSwitch(new[] { 1, 2 }));
WriteLine(CheckSwitch(new[] { 1, 3 }));
WriteLine(CheckSwitch(new[] { 1, 3, 5 }));
WriteLine(CheckSwitch(new[] { 2, 5, 6, 7 }));

WriteLine();

WriteLine(CaptureSlice(new[] { 1, 2, 10 }));
WriteLine(CaptureSlice(new[] { 1, 2, 7, 3, 3, 10 }));
WriteLine(CaptureSlice(new[] { 1, 2 }));
WriteLine(CaptureSlice(new[] { 1, 3 }));
WriteLine(CaptureSlice(new[] { 1, 3, 5 }));
WriteLine(CaptureSlice(new[] { 2, 5, 6, 7 }));


static int CheckSwitch(int[] values)
    => values switch
    {
        [1, 2, .., 10] => 1,
        [1, 2] => 2,
        [1, _] => 3,
        [1, ..] => 4,
        [..] => 50
    };


static string CaptureSlice(int[] values)
    => values switch
    {
        [1, .. var middle, _] => $"Middle {Join(", ", middle)}",
        [.. var all] => $"All {Join(", ", all)}"
    };