/**
 * The InputLineParser grammar.
 *
 * This exist only as an example as the InputLineParser
 * is implemented manually without ANTLR due to its
 * simplicity and to avoid unnecessary dependencies.
 */

input
    : input_part+
    ;

input_part
    : whitespace+
    | single_quoted_string
    | double_quoted_string
    | raw_rest
    | rest
    | input_part_character+
    ;

single_quoted_string
    : '\'' (escape_sequence | ~('\'' | '\\') /* Any character except ' or \ */)* '\''
    ;

double_quoted_string
    : '"' (escape_sequence | ~('"' | '\\') /* Any character except " or \ */)* '"'
    ;

/**
 * Raw consumes the rest of input without attempting
 * to parse any escape sequences.
 */
raw_rest
    : 'rr:' .+
    ;

rest
    : 'r:' (escape_sequence | ~'\\' /* Any character except \ */)*

input_part_character
    : escape_sequence
    | ~(whitespace | '\\') /* Any character except whitespaces or \ */
    ;
    ;

escape_sequence
    : binary_escape_sequence
    | octal_escape_sequence
    | decimal_escape_sequence
    | hexadecimal_escape_sequence
    | simple_escape_sequence
    ;

/******************************************************/
/******************** Atomic rules ********************/
/******************************************************/

binary_escape_sequence
    : '\\b' binary_digit+
    ;

octal_escape_sequence
    : '\\o' octal_digit+
    ;

decimal_escape_sequence
    : '\\' decimal_digit+
    ;

hexadecimal_escape_sequence
    : '\\x' hexadecimal_digit+
    ;

simple_escape_sequence
    : '\\a' | '\\b' | '\\f' | '\\n' | '\\r' | '\\t' | '\\v' | '\\ ' | '\\"' | '\\\'' | '\\\\' | '\\v' | '\\ '
    ;

binary_digit
    : '0' | '1'
    ;

octal_digit
    : '0'..'7'
    ;

decimal_digit
    : '0'..'9'
    ;

hex_digit
    : '0'..'9'
    | 'A'..'F'
    | 'a'..'f';

whitespace
    : '<Any char that passes Char.IsWhitespace>'
    ;