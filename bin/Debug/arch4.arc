<architecture>
	<name>Arch4</name>
	<memory>
		<size>2048</size>
		<au>1</au>
		<rom_start>0</rom_start>
		<rom_end>1023</rom_end>
		<ram_start>1024</ram_start>
		<ram_end>2047</ram_end>
		<init_file>init.ini</init_file>
		<storage_file>memory.mem</storage_file>
	</memory>
	<data>
		<b>8</b>
		<w>16</w>
		<dw>32</dw>
	</data>
	<registers>
		<general_purpose>
			<number>16</number>
			<size>16</size>
			<prefix>R</prefix>
		</general_purpose>
		<bx>
			<size>16</size>
			<name>BX</name> 
			<value>513</value>
			<part>
				<start>0</start>
				<end>7</end>
				<name>BL</name>
			</part>
			<part>
				<start>8</start>
				<end>15</end>
				<name>BH</name>
			</part>
		</bx>
		<pc>
			<size>16</size>
			<name>PC</name>
		</pc>
	</registers>
	<instruction_mnemonics>
		<name>LD</name>
		<name>ST</name>
		<name>ADD</name>
		<name>HALT</name>
	</instruction_mnemonics>
	<addressing_modes>
		<immed>
			<name>immed</name>
			<file>immed.cs</file>
			<result>w</result>
			<expression>"#"IDENTIFIER</expression>
			<expression>"#"NUMBER</expression>
		</immed>		
		<memdir>
			<name>memdir</name>
			<file>memdir.cs</file>
			<result>w</result>
			<expression>IDENTIFIER</expression>
			<expression>NUMBER</expression>
		</memdir>
		<regdir>
			<name>regdir</name>
			<file>regdir.cs</file>
			<result>w</result>
			<expression>
				<registers_group>general_purpose</registers_group>
			</expression>
			<expression_value>
				<expression>"R0"</expression>
				<value>00</value>
			</expression_value>
			<expression_value>
				<expression>"R1"</expression>
				<value>01</value>
			</expression_value>
		</regdir>
		<regind>
			<name>regind</name>
			<file>regind.cs</file>
			<result>w</result>
			<expression>
				"(" <registers_group>general_purpose</registers_group> ")"
			</expression>
			<expression_value>
				<expression>"(R0)"</expression>
				<value>00</value>
			</expression_value>
			<expression_value>
				<expression>"(R1)"</expression>
				<value>01</value>
			</expression_value>
		</regind>
	</addressing_modes>
	<instructions>
		<ld1>
			<mnemonic>LD</mnemonic>
			<size>4</size>
			<file>ld.cs</file>
			<opcode>
				<start_bit>31</start_bit>
				<end_bit>22</end_bit>
				<value>0000000101</value>
			</opcode>
			<arguments>
				<arg>
					<type>dst</type>
					<addressing_mode>
						<name>regdir</name>
						<operand>
							<start_bit>19</start_bit>
							<end_bit>16</end_bit>
						</operand>
					</addressing_mode>
				</arg>
				<arg>
					<type>src</type>
					<addressing_mode>
						<name>memdir</name>
						<opcode>
							<start_bit>21</start_bit>
							<end_bit>20</end_bit>
							<value>01</value>
						</opcode>
						<operand>
							<start_bit>15</start_bit>
							<end_bit>0</end_bit>
						</operand>
					</addressing_mode>
					<addressing_mode>
						<name>immed</name>
						<opcode>
							<start_bit>21</start_bit>
							<end_bit>20</end_bit>
							<value>11</value>
						</opcode>
						<operand>
							<start_bit>15</start_bit>
							<end_bit>0</end_bit>
						</operand>
					</addressing_mode>
				</arg>
			</arguments>
		</ld1>
		<ld2>
			<mnemonic>LD</mnemonic>
			<size>2</size>
			<file>ld.cs</file>
			<opcode>
				<start_bit>11</start_bit>
				<end_bit>8</end_bit>
				<value>0000</value>
			</opcode>
			<arguments>
				<arg>
					<type>dst</type>
					<addressing_mode>
						<name>regdir</name>
						<operand>
							<start_bit>7</start_bit>
							<end_bit>4</end_bit>
						</operand>
					</addressing_mode>
				</arg>
				<arg>
					<type>src</type>
					<addressing_mode>
						<name>regind</name>
						<operand>
							<start_bit>3</start_bit>
							<end_bit>0</end_bit>
						</operand>
					</addressing_mode>
				</arg>
			</arguments>
		</ld2>
	</instructions>
</architecture>