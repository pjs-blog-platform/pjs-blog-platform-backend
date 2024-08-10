using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Posts.API.Database;
using Posts.API.Models;
using Posts.API.Models.DTO;
using Posts.API.Services;

namespace Posts.API.Tests
{
    public class PostServiceTests
    {
        private readonly Mock<PostsContext> _mockContext;
        private readonly Mock<IPostMapper> _mockMapper;
        private readonly PostService _postService;

        public PostServiceTests()
        {
            _mockContext = new Mock<PostsContext>(new DbContextOptions<PostsContext> { });
            _mockMapper = new Mock<IPostMapper>();
            _postService = new PostService(_mockContext.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAsync_WhenCalled_ReturnsAllPosts()
        {
            // Arrange
            var post1 = new Post
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                IsActive = true,
            };

            var post2 = new Post
            {
                Id = 2,
                AuthorId = 2,
                Title = "Test Title 2",
                Content = "Test Content 2",
                Excerpt = "Test Excerpt 2",
                Slug = "test-title-2",
                FeaturedImageUrl = "https://example.com/images/test-image-2.jpg",
                CreatedAt = new DateTime(2024, 7, 29, 11, 30, 00),
                UpdatedAt = new DateTime(2024, 7, 29, 11, 30, 00),
                IsActive = false,
            };

            var post3 = new Post
            {
                Id = 3,
                AuthorId = 3,
                Title = "Test Title 3",
                Content = "Test Content 3",
                Excerpt = "Test Excerpt 3",
                Slug = "test-title-3",
                FeaturedImageUrl = "https://example.com/images/test-image-3.jpg",
                CreatedAt = new DateTime(2024, 7, 30, 12, 45, 00),
                UpdatedAt = new DateTime(2024, 7, 30, 12, 45, 00),
                IsActive = true,
            };

            var postDTO1 = new PostDTO
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = "2024-07-28T10:00:00.0000000",
                UpdatedAt = "2024-07-28T10:00:00.0000000"
            };

            var postDTO2 = new PostDTO
            {
                Id = 2,
                AuthorId = 2,
                Title = "Test Title 2",
                Content = "Test Content 2",
                Excerpt = "Test Excerpt 2",
                Slug = "test-title-2",
                FeaturedImageUrl = "https://example.com/images/test-image-2.jpg",
                CreatedAt = "2024-07-29T11:30:00.0000000",
                UpdatedAt = "2024-07-29T11:30:00.0000000"
            };

            var postDTO3 = new PostDTO
            {
                Id = 3,
                AuthorId = 3,
                Title = "Test Title 3",
                Content = "Test Content 3",
                Excerpt = "Test Excerpt 3",
                Slug = "test-title-3",
                FeaturedImageUrl = "https://example.com/images/test-image-3.jpg",
                CreatedAt = "2024-07-30T12:45:00.0000000",
                UpdatedAt = "2024-07-30T12:45:00.0000000"
            };


            _mockContext.Setup(c => c.Posts).ReturnsDbSet(new List<Post> { post1, post2, post3 }.AsQueryable());
            _mockMapper.Setup(m => m.Map(post1)).Returns(postDTO1);
            _mockMapper.Setup(m => m.Map(post2)).Returns(postDTO2);
            _mockMapper.Setup(m => m.Map(post3)).Returns(postDTO3);

            // Act
            var result = await _postService.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());

            var result1 = result.ToList()[0];
            var result2 = result.ToList()[1];
            var result3 = result.ToList()[2];

            // Assertions for postDTO1
            Assert.Equal(postDTO1.Id, result1.Id);
            Assert.Equal(postDTO1.AuthorId, result1.AuthorId);
            Assert.Equal(postDTO1.Title, result1.Title);
            Assert.Equal(postDTO1.Content, result1.Content);
            Assert.Equal(postDTO1.Excerpt, result1.Excerpt);
            Assert.Equal(postDTO1.Slug, result1.Slug);
            Assert.Equal(postDTO1.FeaturedImageUrl, result1.FeaturedImageUrl);
            Assert.Equal(postDTO1.CreatedAt, result1.CreatedAt);
            Assert.Equal(postDTO1.UpdatedAt, result1.UpdatedAt);

            // Assertions for postDTO2
            Assert.Equal(postDTO2.Id, result2.Id);
            Assert.Equal(postDTO2.AuthorId, result2.AuthorId);
            Assert.Equal(postDTO2.Title, result2.Title);
            Assert.Equal(postDTO2.Content, result2.Content);
            Assert.Equal(postDTO2.Excerpt, result2.Excerpt);
            Assert.Equal(postDTO2.Slug, result2.Slug);
            Assert.Equal(postDTO2.FeaturedImageUrl, result2.FeaturedImageUrl);
            Assert.Equal(postDTO2.CreatedAt, result2.CreatedAt);
            Assert.Equal(postDTO2.UpdatedAt, result2.UpdatedAt);

            // Assertions for postDTO3
            Assert.Equal(postDTO3.Id, result3.Id);
            Assert.Equal(postDTO3.AuthorId, result3.AuthorId);
            Assert.Equal(postDTO3.Title, result3.Title);
            Assert.Equal(postDTO3.Content, result3.Content);
            Assert.Equal(postDTO3.Excerpt, result3.Excerpt);
            Assert.Equal(postDTO3.Slug, result3.Slug);
            Assert.Equal(postDTO3.FeaturedImageUrl, result3.FeaturedImageUrl);
            Assert.Equal(postDTO3.CreatedAt, result3.CreatedAt);
            Assert.Equal(postDTO3.UpdatedAt, result3.UpdatedAt);

        }

        [Fact]
        public async Task GetAsync_NoPosts_ReturnsEmptyList()
        {
            // Arrange
            _mockContext.Setup(c => c.Posts).ReturnsDbSet(new List<Post>().AsQueryable());

            // Act
            var result = await _postService.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdAsync_PostExists_ReturnsUser()
        {
            // Arrange
            var post = new Post
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                IsActive = true,
            };


            var postDTO = new PostDTO
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = "2024-07-28T10:00:00.0000000",
                UpdatedAt = "2024-07-28T10:00:00.0000000"
            };

            _mockContext.Setup(c => c.Posts).ReturnsDbSet(new List<Post> { post }.AsQueryable());
            _mockMapper.Setup(m => m.Map(post)).Returns(postDTO);

            // Act
            var result = await _postService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(postDTO.Id, result.Id);
            Assert.Equal(postDTO.AuthorId, result.AuthorId);
            Assert.Equal(postDTO.Title, result.Title);
            Assert.Equal(postDTO.Content, result.Content);
            Assert.Equal(postDTO.Excerpt, result.Excerpt);
            Assert.Equal(postDTO.Slug, result.Slug);
            Assert.Equal(postDTO.FeaturedImageUrl, result.FeaturedImageUrl);
            Assert.Equal(postDTO.CreatedAt, result.CreatedAt);
            Assert.Equal(postDTO.UpdatedAt, result.UpdatedAt);
        }

        [Fact]
        public async Task GetByIdAsync_PostDoesNotExist_ReturnsNull()
        {
            // Arrange
            _mockContext.Setup(c => c.Posts).ReturnsDbSet(new List<Post>().AsQueryable());

            // Act
            var result = await _postService.GetByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ValidCreatePostDTO_ReturnsPostDTO()
        {
            // Arrange
            var createPostDTO = new CreatePostDTO
            {
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
            };

            var post = new Post
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                IsActive = true,
            };

            var postDTO = new PostDTO
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = "2024-07-28T10:00:00.0000000",
                UpdatedAt = "2024-07-28T10:00:00.0000000"
            };

            // Create a mock DbSet<User>
            var mockSet = new Mock<DbSet<Post>>();

            mockSet.Setup(m => m.Add(It.IsAny<Post>())).Callback<Post>(u =>
            {
                u.Id = 1;
            });

            _mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Mock the mapper
            _mockMapper.Setup(m => m.Map(It.IsAny<Post>())).Returns((Post p) =>
            {
                postDTO.Id = p.Id;
                return postDTO;
            }).Verifiable();

            // Act
            var result = await _postService.CreateAsync(createPostDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(postDTO.Id, result.Id);
            Assert.Equal(postDTO.AuthorId, result.AuthorId);
            Assert.Equal(postDTO.Title, result.Title);
            Assert.Equal(postDTO.Content, result.Content);
            Assert.Equal(postDTO.Excerpt, result.Excerpt);
            Assert.Equal(postDTO.Slug, result.Slug);
            Assert.Equal(postDTO.FeaturedImageUrl, result.FeaturedImageUrl);
            Assert.Equal(postDTO.CreatedAt, result.CreatedAt);
            Assert.Equal(postDTO.UpdatedAt, result.UpdatedAt);

            // Verify that the mapper was called once
            _mockMapper.Verify(m => m.Map(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_NullCreatePostDTO_ThrowsArgumentNullException()
        {
            // Arrange
            CreatePostDTO nullCreatePostDTO = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _postService.CreateAsync(nullCreatePostDTO));
        }

        [Fact]
        public async Task CreateAsync_NullAuthorId_ThrowsArgumentException()
        {
            // Arrange
            var createPostDTO = new CreatePostDTO
            {
                AuthorId = null,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _postService.CreateAsync(createPostDTO));
        }

        [Fact]
        public async Task CreateAsync_NullTitle_ThrowsArgumentException()
        {
            // Arrange
            var createPostDTO = new CreatePostDTO
            {
                AuthorId = 1,
                Title = null,
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _postService.CreateAsync(createPostDTO));
        }

        [Fact]
        public async Task CreateAsync_EmptyTitle_ThrowsArgumentException()
        {
            // Arrange
            var createPostDTO = new CreatePostDTO
            {
                AuthorId = 1,
                Title = String.Empty,
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _postService.CreateAsync(createPostDTO));
        }

        [Fact]
        public async Task CreateAsync_NullContent_ThrowsArgumentException()
        {
            // Arrange
            var createPostDTO = new CreatePostDTO
            {
                AuthorId = 1,
                Title = "Test Title 1",
                Content = null,
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _postService.CreateAsync(createPostDTO));
        }

        [Fact]
        public async Task CreateAsync_EmptyContent_ThrowsArgumentException()
        {
            // Arrange
            var createPostDTO = new CreatePostDTO
            {
                AuthorId = 1,
                Title = "Test Title 1",
                Content = String.Empty,
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _postService.CreateAsync(createPostDTO));
        }

        [Fact]
        public async Task CreateAsync_NullSlug_ThrowsArgumentException()
        {
            // Arrange
            var createPostDTO = new CreatePostDTO
            {
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = null,
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _postService.CreateAsync(createPostDTO));
        }

        [Fact]
        public async Task CreateAsync_EmptySlug_ThrowsArgumentException()
        {
            // Arrange
            var createPostDTO = new CreatePostDTO
            {
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Content 1",
                Slug = String.Empty,
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _postService.CreateAsync(createPostDTO));
        }

        [Fact]
        public async Task UpdateAsync_ValidPost_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updatePost = new UpdatePostDTO
            {
                Title = "Updated Title",
                Content = "Updated Content",
                Excerpt = "Updated Excerpt",
                Slug = "Updated Slug",
                FeaturedImageUrl = "https://example.com/updated-featured-image.jpg"
            };

            var existingPost = new Post
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                IsActive = true,
            };

            var posts = new List<Post> { existingPost }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            _mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _postService.UpdateAsync(userId, updatePost);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.Equal(updatePost.Title, existingPost.Title);
            Assert.Equal(updatePost.Content, existingPost.Content);
            Assert.Equal(updatePost.Excerpt, existingPost.Excerpt);
            Assert.Equal(updatePost.Slug, existingPost.Slug);
            Assert.Equal(updatePost.FeaturedImageUrl, existingPost.FeaturedImageUrl);
        }

        [Fact]
        public async Task UpdateAsync_PostDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var userId = 999; // ID that doesn't exist
            var updatePost = new UpdatePostDTO
            {
                Title = "Updated Title",
                Content = "Updated Content",
                Excerpt = "Updated Excerpt",
                Slug = "Updated Slug",
                FeaturedImageUrl = "https://example.com/updated-featured-image.jpg"
            };

            var posts = new List<Post>().AsQueryable(); // Empty list of users

            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            _mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            // Act
            var result = await _postService.UpdateAsync(userId, updatePost);

            // Assert
            Assert.False(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_NullTitle_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updatePost = new UpdatePostDTO
            {
                Title = null,
                Content = "Updated Content",
                Excerpt = "Updated Excerpt",
                Slug = "Updated Slug",
                FeaturedImageUrl = "https://example.com/updated-featured-image.jpg"
            };

            var existingPost = new Post
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                IsActive = true,
            };

            var posts = new List<Post> { existingPost }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            _mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _postService.UpdateAsync(userId, updatePost);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.NotNull(existingPost.Title);
            Assert.Equal(updatePost.Content, existingPost.Content);
            Assert.Equal(updatePost.Excerpt, existingPost.Excerpt);
            Assert.Equal(updatePost.Slug, existingPost.Slug);
            Assert.Equal(updatePost.FeaturedImageUrl, existingPost.FeaturedImageUrl);
        }

        [Fact]
        public async Task UpdateAsync_EmptyTitle_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updatePost = new UpdatePostDTO
            {
                Title = String.Empty,
                Content = "Updated Content",
                Excerpt = "Updated Excerpt",
                Slug = "Updated Slug",
                FeaturedImageUrl = "https://example.com/updated-featured-image.jpg"
            };

            var existingPost = new Post
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                IsActive = true,
            };

            var posts = new List<Post> { existingPost }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            _mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _postService.UpdateAsync(userId, updatePost);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.NotEmpty(existingPost.Title);
            Assert.Equal(updatePost.Content, existingPost.Content);
            Assert.Equal(updatePost.Excerpt, existingPost.Excerpt);
            Assert.Equal(updatePost.Slug, existingPost.Slug);
            Assert.Equal(updatePost.FeaturedImageUrl, existingPost.FeaturedImageUrl);
        }

        [Fact]
        public async Task UpdateAsync_NullContent_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updatePost = new UpdatePostDTO
            {
                Title = "Updated Title",
                Content = null,
                Excerpt = "Updated Excerpt",
                Slug = "Updated Slug",
                FeaturedImageUrl = "https://example.com/updated-featured-image.jpg"
            };

            var existingPost = new Post
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                IsActive = true,
            };

            var posts = new List<Post> { existingPost }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            _mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _postService.UpdateAsync(userId, updatePost);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.Equal(updatePost.Title, existingPost.Title);
            Assert.NotNull(existingPost.Title);
            Assert.Equal(updatePost.Excerpt, existingPost.Excerpt);
            Assert.Equal(updatePost.Slug, existingPost.Slug);
            Assert.Equal(updatePost.FeaturedImageUrl, existingPost.FeaturedImageUrl);
        }

        [Fact]
        public async Task UpdateAsync_EmptyContent_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updatePost = new UpdatePostDTO
            {
                Title = "Updated Title",
                Content = String.Empty,
                Excerpt = "Updated Excerpt",
                Slug = "Updated Slug",
                FeaturedImageUrl = "https://example.com/updated-featured-image.jpg"
            };

            var existingPost = new Post
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                IsActive = true,
            };

            var posts = new List<Post> { existingPost }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            _mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _postService.UpdateAsync(userId, updatePost);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.Equal(updatePost.Title, existingPost.Title);
            Assert.NotEmpty(existingPost.Title);
            Assert.Equal(updatePost.Excerpt, existingPost.Excerpt);
            Assert.Equal(updatePost.Slug, existingPost.Slug);
            Assert.Equal(updatePost.FeaturedImageUrl, existingPost.FeaturedImageUrl);
        }

        [Fact]
        public async Task UpdateAsync_NullExcerpt_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updatePost = new UpdatePostDTO
            {
                Title = "Updated Title",
                Content = "Updated Content",
                Excerpt = null,
                Slug = "Updated Slug",
                FeaturedImageUrl = "https://example.com/updated-featured-image.jpg"
            };

            var existingPost = new Post
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                IsActive = true,
            };

            var posts = new List<Post> { existingPost }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            _mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _postService.UpdateAsync(userId, updatePost);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.Equal(updatePost.Title, existingPost.Title);
            Assert.Equal(updatePost.Content, existingPost.Content);
            Assert.NotNull(existingPost.Excerpt);
            Assert.Equal(updatePost.Slug, existingPost.Slug);
            Assert.Equal(updatePost.FeaturedImageUrl, existingPost.FeaturedImageUrl);
        }

        [Fact]
        public async Task UpdateAsync_EmptyExcerpt_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updatePost = new UpdatePostDTO
            {
                Title = "Updated Title",
                Content = "Updated Content",
                Excerpt = String.Empty,
                Slug = "Updated Slug",
                FeaturedImageUrl = "https://example.com/updated-featured-image.jpg"
            };

            var existingPost = new Post
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                IsActive = true,
            };

            var posts = new List<Post> { existingPost }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            _mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _postService.UpdateAsync(userId, updatePost);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.Equal(updatePost.Title, existingPost.Title);
            Assert.Equal(updatePost.Content, existingPost.Content);
            Assert.NotEmpty(existingPost.Excerpt);
            Assert.Equal(updatePost.Slug, existingPost.Slug);
            Assert.Equal(updatePost.FeaturedImageUrl, existingPost.FeaturedImageUrl);
        }

        [Fact]
        public async Task UpdateAsync_NullSlug_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updatePost = new UpdatePostDTO
            {
                Title = "Updated Title",
                Content = "Updated Content",
                Excerpt = "Updated Excerpt",
                Slug = null,
                FeaturedImageUrl = "https://example.com/updated-featured-image.jpg"
            };

            var existingPost = new Post
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                IsActive = true,
            };

            var posts = new List<Post> { existingPost }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            _mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _postService.UpdateAsync(userId, updatePost);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.Equal(updatePost.Title, existingPost.Title);
            Assert.Equal(updatePost.Content, existingPost.Content);
            Assert.Equal(updatePost.Excerpt, existingPost.Excerpt);
            Assert.NotNull(existingPost.Slug);
            Assert.Equal(updatePost.FeaturedImageUrl, existingPost.FeaturedImageUrl);
        }

        [Fact]
        public async Task UpdateAsync_EmptySlug_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updatePost = new UpdatePostDTO
            {
                Title = "Updated Title",
                Content = "Updated Excerpt",
                Excerpt = "Updated Excerpt",
                Slug = String.Empty,
                FeaturedImageUrl = "https://example.com/updated-featured-image.jpg"
            };

            var existingPost = new Post
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                IsActive = true,
            };

            var posts = new List<Post> { existingPost }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            _mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _postService.UpdateAsync(userId, updatePost);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.Equal(updatePost.Title, existingPost.Title);
            Assert.Equal(updatePost.Content, existingPost.Content);
            Assert.Equal(updatePost.Excerpt, existingPost.Excerpt);
            Assert.NotEmpty(existingPost.Slug);
            Assert.Equal(updatePost.FeaturedImageUrl, existingPost.FeaturedImageUrl);
        }

        [Fact]
        public async Task UpdateAsync_NullFeaturedImageUrl_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updatePost = new UpdatePostDTO
            {
                Title = "Updated Title",
                Content = "Updated Content",
                Excerpt = "Updated Excerpt",
                Slug = "Updated Slug",
                FeaturedImageUrl = null
            };

            var existingPost = new Post
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                IsActive = true,
            };

            var posts = new List<Post> { existingPost }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            _mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _postService.UpdateAsync(userId, updatePost);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.Equal(updatePost.Title, existingPost.Title);
            Assert.Equal(updatePost.Content, existingPost.Content);
            Assert.Equal(updatePost.Excerpt, existingPost.Excerpt);
            Assert.Equal(updatePost.Slug, existingPost.Slug);
            Assert.NotNull(existingPost.FeaturedImageUrl);
        }

        [Fact]
        public async Task UpdateAsync_EmptyFeaturedImageUrl_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updatePost = new UpdatePostDTO
            {
                Title = "Updated Title",
                Content = "Updated Excerpt",
                Excerpt = "Updated Excerpt",
                Slug = "Updated Slug",
                FeaturedImageUrl = String.Empty
            };

            var existingPost = new Post
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                IsActive = true,
            };

            var posts = new List<Post> { existingPost }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            _mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _postService.UpdateAsync(userId, updatePost);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.Equal(updatePost.Title, existingPost.Title);
            Assert.Equal(updatePost.Content, existingPost.Content);
            Assert.Equal(updatePost.Excerpt, existingPost.Excerpt);
            Assert.Equal(updatePost.Slug, existingPost.Slug);
            Assert.NotEmpty(existingPost.FeaturedImageUrl);
        }

        [Fact]
        public async Task DeleteAsync_ValidPostId_ReturnsTrue()
        {
            // Arrange
            var postId = 1;
            var existingPost = new Post
            {
                Id = 1,
                AuthorId = 1,
                Title = "Test Title 1",
                Content = "Test Content 1",
                Excerpt = "Test Excerpt 1",
                Slug = "test-title-1",
                FeaturedImageUrl = "https://example.com/images/test-image-1.jpg",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00),
                IsActive = true,
            };

            var posts = new List<Post> { existingPost }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            _mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _postService.DeleteAsync(postId);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_UserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var postId = 1;
            var posts = new List<Post>().AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.GetEnumerator());

            _mockContext.Setup(c => c.Posts).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _postService.DeleteAsync(postId);

            // Assert
            Assert.False(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }
    }
}