
use std::collections::HashMap;
use std::env;
use std::path::{Path, PathBuf};
use std::io::{self, Read};

use syntax::ast;
use syntax::codemap::{CodeMap, Loc, Span};
use syntax::errors::DiagnosticBuilder;
use syntax::parse::{self, ParseSess};
use syntax::visit::{self, FnKind, Visitor};

pub struct Indexer
{
    source : PathBuf,
    functions : HashMap<String, String>,
    codemap : CodeMap
}

impl Indexer
{
    pub fn new(source : PathBuf) -> Indexer {
        Indexer {
            source: source,
            functions: HashMap::new(),
            codemap: CodeMap::new()
        }
    }

    pub fn run(&mut self, session : &ParseSess) -> Option<HashMap<String,String>> {

        self.functions.clear();
        parse::parse_crate_from_file(
                &self.source,
                vec!(),
                session).ok()

            .map(|parsed_crate| self.visit_mod(&parsed_crate.module, parsed_crate.span, 0))
            .map(|_| self.functions.clone())
    }
}


impl Visitor for Indexer
{
    fn visit_fn(&mut self,
                fn_kind: FnKind,
                fn_decl: & ast::FnDecl,
                block: & ast::Block,
                span: Span,
                _id: ast::NodeId) {
        let fn_name = match fn_kind {
            FnKind::ItemFn(id, _, _, _, _, _) |
            FnKind::Method(id, _, _) => id.name.as_str().to_string(),
            FnKind::Closure => format!("<closure at ?>" /*, self.format_span(span)*/ ),
        };

        //self.arg_counts.insert(fn_name.clone(), fn_decl.inputs.len());

        println!("Found fn: {:?}", fn_decl);
        self.functions.insert(fn_name, format!("{:?}", fn_decl));

        // Continue walking the rest of the funciton so we pick up any functions
        // or closures defined in its body.
        visit::walk_fn(self, fn_kind, fn_decl, block, span);
    }

    // The default implementation panics, so this is needed to work on files
    // with macro invocations, eg calls to `format!()` above. A better solution
    // would be to expand macros before walking the AST, but I haven't looked at
    // how to do that. We will miss any functions defined via a macro, but
    // that's fine for this example.
    fn visit_mac(&mut self, _mac: & ast::Mac) {}
}