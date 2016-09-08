#![feature(rustc_private)]

extern crate syntax;

use syntax::parse::*;
use syntax::ast::*;

fn main() {
    println!("Hello world!");
    let session = ParseSess::new();
    let crateConfig = vec!();

    std::env::current_exe()
        // call parser
        .map(|mut path| {
            path.pop();
            path.pop();
            path.pop();
            path.push("project");
            path.push("test.rs");

            let mut p = new_parser_from_file(&session,  crateConfig, &path);
            let result = p.parse_fn_decl(true);
            println!("done: {:?}", result);
        });
}

