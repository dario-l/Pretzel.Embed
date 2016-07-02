# Pretzel.Embed

Allows you to embed soical posts from **Twitter**, **Instagram**, **VK**, **Facebook** and **Youtube**. 

This is a plugin for the static site generation tool [Pretzel](https://github.com/Code52/pretzel).

### Usage

#### Twitter

```
{% embed twitter "https://twitter.com/sergun/status/671315150796939264" %}
Завести веб-приложение <a href="https://t.co/P2zd8MiA9y">https://t.co/P2zd8MiA9y</a> внутри Mindstorms EV3? Да пожалуйста! :-) <a href="https://t.co/Lf6Yw49lWw">https://t.co/Lf6Yw49lWw</a>
{% endembed %}
```

#### Instagram

```
{% embed instagram "https://www.instagram.com/p/BCvXFxwGOyg" %}
Первое знакомство с ROBOTC for Mindstorms. Первый осознанный код на C. Исторический момент :-)
{% endembed %}
```

#### VK

```
{% embed vk "vk_post_2593705_1050" %}{% endembed %}
```

#### Facebook

```
{% embed facebook "https%3A%2F%2Fwww.facebook.com%2Fsergeyzwezdin%2Fposts%2F10154249249067970" 260 %}{% endembed %}
```

#### Youtbue

```
{% embed youtube "AAVwgIDQxgU" %}{% endembed %}
```

### Installation

Download the [latest release](https://github.com/sergeyzwezdin/Pretzel.Embed/releases/latest) and copy `Pretzel.Embed.dll` to the `_plugins` folder at the root of your site folder.