Vagrant.configure("2") do |config|
  # ---------- Ubuntu VM ----------
  config.vm.box = "ubuntu/jammy64"
  config.vm.synced_folder "C:/Users/nikos/source/repos/OAuthMvcSample", "/vagrant"
  config.vm.hostname = "oauth-ubuntu"

  # Проброс портів для доступу до сайту з хост-машини
  config.vm.network "forwarded_port", guest: 5000, host: 5000
  config.vm.network "forwarded_port", guest: 5001, host: 5001

  # Таймаут очікування завантаження (10 хв)
  config.vm.boot_timeout = 600

  # Ресурси для ВМ
  config.vm.provider "virtualbox" do |vb|
    vb.memory = "4096"
    vb.cpus = 2
    vb.name = "OAuthMvcSample_Ubuntu"
  end

  # ---------- Provisioning ----------
  config.vm.provision "shell", inline: <<-SHELL
    echo "=== Оновлення системи ==="
    sudo apt update -y

    echo "=== Встановлення залежностей ==="
    sudo apt install -y apt-transport-https ca-certificates gnupg wget

    echo "=== Підключення репозиторію Microsoft ==="
    wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    sudo apt update -y

    echo "=== Встановлення .NET SDK 8.0 ==="
    sudo apt install -y dotnet-sdk-8.0

    echo "=== Перевірка встановлення .NET ==="
    dotnet --info

    echo "=== Запуск ASP.NET Core застосунку ==="
    cd /vagrant/OAuthMvcSample
    dotnet restore
    nohup dotnet run --urls "https://0.0.0.0:5001" > /home/vagrant/app.log 2>&1 &
    echo "=== Застосунок запущено ==="
  SHELL
end