
var test = false;
var number1 = 2;
var number2 = 3;


_ = $"interpolated string with value {(
        test ?
            number1
            :
            number2
    )}, and some more text...";