use strict;
use WWW::Scripter;

my $script = new WWW::Scripter;
$script->use_plugin('JavaScript');

my $username = $ARGV[0];
my $password = $ARGV[1]; 

my $url = 'https://customer.comcast.com/Secure/UsageMeterDetail.aspx';

$script->get($url);

$script->submit_form(
    form_name => 'signin',
    fields      => {
        user    => $username,
        passwd  => $password,
    }
);

#$mech->submit_form( form_name => 'aspnetForm');

# some reason this is the loading text rather than the correct text :<
print $script->text();

