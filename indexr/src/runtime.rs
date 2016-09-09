
use std::io::{self, Read};
use std::path::{Path, PathBuf};
use syntax::parse::{self, ParseSess};

use super::Indexer;

pub struct Runtime
{
    indexer : Indexer
}

trait Effect<A>
{
    fn effect<F : FnOnce(&A)>(&self, f : F) -> &Self;
}

impl<A> Effect<A> for Option<A>
{
    fn effect<F : FnOnce(&A)>(&self, f : F) -> &Self{
        match self {
            &Some(ref x) => { f(&x); self},
            &None    => self
        }
    }
}

impl Runtime
{
    pub fn new(source : PathBuf) -> Runtime {
        Runtime {
            indexer: Indexer::new(source)
        }
    }

    pub fn run(&mut self) {
        loop {
            let mut buffer = String::new();
            io::stdin()
                .read_line(&mut buffer).ok()

                .effect(|n| println!("read {} bytes: {:?}", n, buffer))

                .and_then(|_| self.handle_command(buffer));
        }
    }

    fn handle_command(&mut self, command : String) -> Option<()> {
        print!("found: {:?}", command);
            match command.as_ref() {
                "get\n" => {
                    println!("found get");
                    let session = ParseSess::new();
                    self.indexer.run(&session)
                                     .map(|functions| println!("{:?}", functions)) },
                _     => None } }
}

