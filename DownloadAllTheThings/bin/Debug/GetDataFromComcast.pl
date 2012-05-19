use strict;
use WWW::Mechanize;

my $mech = WWW::Mechanize->new();

my $username = $ARGV[0];
my $password = $ARGV[1]; 


my $url = 'https://customer.comcast.com/Secure/UsageMeterDetail.aspx';

$mech->get($url);

$mech->submit_form(
    form_name => 'signin',
    fields      => {
        user    => $username,
        passwd  => $password,
        check	=> 1,
    }
);

$mech->submit_form( form_name => 'redir' );


# some reason this is the loading text rather than the correct text :<
print $mech->text();
