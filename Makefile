start: sent/sent
	sent/sent generics.txt

clean:
	$(MAKE) -C sent clean

sent/sent: sent/sent.d
	$(MAKE) -C sent

sent/sent.d:
	@echo "can't"
