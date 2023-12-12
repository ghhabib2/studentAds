using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using System.Data.Entity.Core;
using System.Data.Entity.Migrations;

namespace Classified.Data.Base
{
    /// <summary>
    /// Generic Interface for Make the relations between Source View Model and Destination database Object
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="D"></typeparam>
    public interface IRepositoryBase<S, D>
    {
        /// <summary>
        /// Insert Many Entities in the same time
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool InsertMany(IEnumerable<S> entities);

        /// <summary>
        /// Insert new record into database
        /// </summary>
        /// <param name="entity">Entity in the format of Source Class</param>
        bool Insert(S entity);

        /// <summary>
        /// Insert the new record into database and return destination object as the result.
        /// </summary>
        /// <param name="entity">Entity in the format of Source Class</param>
        S InsertByReturningTargetObject(S entity);

        /// <summary>
        /// Update information of database based
        /// </summary>
        /// <param name="entity">The entity in the View Model format</param>
        /// <param name="where">Query expression for destination</param>
        /// <returns>True if the operation is successful and false in either case</returns>
        bool Update(S entity, Expression<Func<D, bool>> where);

        /// <summary>
        /// Delete a selected object from Database
        /// </summary>
        /// <param name="where">Query expression for destination</param>
        /// <returns>True if the operation is successful and false in either case</returns>
        bool Delete(Expression<Func<D, bool>> where);

        /// <summary>
        /// Get a View Model based on the Source Type by query the Destination
        /// </summary>
        /// <param name="where">Query expression for destination</param>
        /// <returns>Object in Source View Model Format</returns>
        S Get(Expression<Func<D, bool>> where);

        /// <summary>
        /// Get all the records in database based on the Source View Model Format
        /// </summary>
        /// <returns>List of all records in Source ViewModel Format</returns>
        IEnumerable<S> GetAll();

        /// <summary>
        /// Get filtered list of information in viewModel Format
        /// </summary>
        /// <param name="where">Query expression for destination</param>
        /// <returns>Filtered list of records in View Model Format</returns>
        IEnumerable<S> GetMany(Expression<Func<D, bool>> where);


    }

    /// <summary>
    /// Generic Class for Make the relations between Source View Model and Destination database Object
    /// </summary>
    /// <typeparam name="S">View Model Class</typeparam>
    /// <typeparam name="D">Database object entity Class</typeparam>
    public abstract class RepositoryBase<S, D> : DatabaseFactory, IRepositoryBase<S, D>
    where S : class, new()
    where D : class, new()
    {
        public virtual bool InsertMany(IEnumerable<S> entities)
        {

            foreach (var item in entities)
            {
                //Map the Source to The Destination
                var tempDestination = Mapper.Map<S, D>(item);

                //Add the object in the form of Destination into Database
                Context.Set<D>().Add(tempDestination);

            }

            try
            {
                //Save Changes into Database
                Context.SaveChanges();
                return true;
            }
            catch (EntityException ex)
            {
                //Write the Exception to the Line
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public virtual bool Insert(S entity)
        {

            //Map the Source to The Destination
            var tempDestination = Mapper.Map<S, D>(entity);

            try
            {
                //Add the object in the form of Destination into Database
                Context.Set<D>().Add(tempDestination);


                //Save Changes into Database
                Context.SaveChanges();

                return true;
            }
            catch (EntityException ex)
            {
                //Write the Exception to the Line
                Console.WriteLine(ex.Message);
            }

            return false;
        }


        public virtual S InsertByReturningTargetObject(S entity)
        {

            //Map the Source to The Destination
            var tempDestination = Mapper.Map<S, D>(entity);

            try
            {
                //Add the object in the form of Destination into Database
                Context.Set<D>().Add(tempDestination);


                //Save Changes into Database
                Context.SaveChanges();

                return Mapper.Map<D, S>(tempDestination);
            }
            catch (EntityException ex)
            {
                //Write the Exception to the Line
                Console.WriteLine(ex.Message);
            }

            return null;
        }


        public virtual bool Update(S entity, Expression<Func<D, bool>> where)
        {
            try
            {

                //Find object in database
                var tempDb = Context.Set<D>().SingleOrDefault(where);

                //Check if the object exist in database
                if (tempDb != null)
                {
                    //Map the Source to The Destination
                    var targetEntity = Mapper.Map<S, D>(entity);
                    //Update the Entries
                    Context.Entry(tempDb).CurrentValues.SetValues(targetEntity);

                    //Save Changes into Database
                    Context.SaveChanges();

                    return true;
                }
                else
                {
                    throw new EntityException("There is no record find in the database with the information you asked for");
                }

            }
            catch (EntityException ex)
            {
                //Write the Exception to the Line
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public virtual bool Delete(Expression<Func<D, bool>> where)
        {

            try
            {

                var objects = Context.Set<D>().Where(where).ToList();

                if (objects.Any())
                {
                    foreach (D obj in objects)
                        Context.Set<D>().Remove(obj);
                    Context.SaveChanges();

                    return true;
                }
                else
                {
                    throw new EntityException("There is no record find in the database with the information you asked for");
                }



            }
            catch (EntityException ex)
            {
                //Write the Exception to the Line
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public virtual S Get(Expression<Func<D, bool>> where)
        {
            try
            {

                var dbObject = Context.Set<D>().SingleOrDefault(where);

                if (dbObject != null)
                {
                    return Mapper.Map<D, S>(dbObject);
                }
                else
                {
                    throw new EntityException("There is no record find in the database with the information you asked for");
                }

            }
            catch (EntityException ex)
            {
                //Write the Exception to the Line
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public virtual IEnumerable<S> GetAll()
        {
            try
            {
                return Context.Set<D>().ToList().Select(Mapper.Map<D, S>);
            }
            catch (EntityException ex)
            {
                //Write the Exception to the Line
                Console.WriteLine(ex.Message);
            }

            return new List<S>();
        }


        public virtual IEnumerable<S> GetMany(Expression<Func<D, bool>> where)
        {
            try
            {
                var tempObjects = Context.Set<D>().Where(where).ToList().Select(Mapper.Map<D, S>).ToList();

                if (tempObjects.Any())
                    return tempObjects.ToList();
                else
                    throw new EntityException("There is no record find in the database with the information you asked for");

            }
            catch (EntityException ex)
            {
                //Write the Exception to the Line
                Console.WriteLine(ex.Message);
            }

            return new List<S>();

        }

    }
}
